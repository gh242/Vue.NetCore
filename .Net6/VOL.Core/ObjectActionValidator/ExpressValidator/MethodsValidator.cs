using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using VOL.Core.Extensions;
using VOL.Core.Filters;
using VOL.Core.Utilities;


namespace VOL.Core.ObjectActionValidator
{
    public static class MethodsValidator
    {
        /// <summary>
        /// 方法上的model校驗配置
        /// </summary>
        public static Dictionary<string, string[]> ValidatorCollection { get; } = new Dictionary<string, string[]>();

        public static void Add<T>(this ValidatorModel validatorGroup, Expression<Func<T, object>> loginExpress = null)
        {
            if (!ValidatorCollection.TryAdd(validatorGroup.ToString().ToLower(),
                loginExpress == null
                ? typeof(T).GetGenericProperties().Select(x => x.Name).ToArray()
                : loginExpress.GetExpressionToArray()))
            {
                throw new Exception($"鍵{validatorGroup.ToString()}的表達式已經註冊過了");
            }
        }
        /// <summary>
        /// 方法上的普通參數校驗配置
        /// </summary>
        public static Dictionary<string, GeneralOptions> ValidatorGeneralCollection { get; } = new Dictionary<string, GeneralOptions>();

        /// <summary>
        /// 默認校驗的是string類型
        /// </summary>
        /// <param name="general">普通參數名</param>
        /// <param name="CNName">校驗錯誤時顯示的提示名字</param>
        public static void Add(this ValidatorGeneral general, string CNName)
        {
            general.Add(CNName, ParamType.String, null, null);
        }
        public static void Add(this ValidatorGeneral general, string CNName, int? max)
        {
            general.Add(CNName, ParamType.String, null, max);
        }
        public static void Add(this ValidatorGeneral general, string CNName, ParamType type)
        {
            general.Add(CNName, type, null, null);
        }
        public static void Add(this ValidatorGeneral general, string CNName, ParamType type, int? max)
        {
            general.Add(CNName, type, null, max);
        }
        public static void Add(this ValidatorGeneral general, string CNName, int? min, int? max)
        {
            general.Add(CNName, ParamType.String, min, max);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="general">普通參數名</param>
        /// <param name="CNName">校驗錯誤時顯示的提示名字</param>
        /// <param name="type">參數類型</param>
        /// <param name="min">最大小度(最小值)</param>
        /// <param name="max">最大長度(最大值)</param>
        public static void Add(this ValidatorGeneral general, string CNName, ParamType type, int? min = null, int? max = null)
        {
            GeneralOptions options = new GeneralOptions(general, CNName, type, min, max);
            if (!ValidatorGeneralCollection.TryAdd(general.ToString().ToLower(), options))
            {
                throw new Exception($"鍵{general.ToString()}參數配置已經注入過了");
            }
        }
        /// <summary>
        /// 自定義驗證
        /// </summary>
        /// <param name="general"></param>
        /// <param name="customValidator"></param>
        public static void Add(this ValidatorGeneral general, string CNName, Func<object, ObjectValidatorResult> customValidator)
        {
            GeneralOptions options = new GeneralOptions(general, CNName, customValidator);
            if (!ValidatorGeneralCollection.TryAdd(general.ToString().ToLower(), options))
            {
                throw new Exception($"鍵{general.ToString()}參數配置已經注入過了");
            }
        }



        /// <summary>
        /// 獲取方法上綁定的model校驗字段
        /// </summary>
        /// <param name="validatorGroup"></param>
        /// <returns></returns>
        public static string[] GetModelParameters(this ValidatorModel validatorGroup)
        {
            return validatorGroup.ToString().GetModelParameters();
        }
        /// <summary>
        /// 獲取方法上綁定的model校驗字段
        /// </summary>
        /// <param name="validatorGroup"></param>
        /// <returns></returns>
        public static string[] GetModelParameters(this string validatorGroup)
        {
            if (!ValidatorCollection.TryGetValue(validatorGroup.ToLower(), out string[] values))
            {
                Console.WriteLine($"未註冊{validatorGroup}參數的表達式");
                throw new Exception($"未註冊{validatorGroup}參數的表達式");
            }
            return values;
        }
        /// <summary>
        /// 獲取方法上綁定校驗字段的配置信息
        /// </summary>
        /// <param name="general"></param>
        /// <returns></returns>
        public static IEnumerable<GeneralOptions> GetGeneralOption(this ValidatorGeneral[] general)
        {
            return general.Select(x => x.ToString()).ToArray().GetGeneralOption();
        }
        /// <summary>
        /// 獲取方法上綁定校驗字段的配置信息
        /// </summary>
        /// <param name="general"></param>
        /// <returns></returns>
        public static IEnumerable<GeneralOptions> GetGeneralOption(this string[] generalName)
        {
            foreach (string item in generalName)
            {
                if (!ValidatorGeneralCollection.TryGetValue(item.ToLower(), out GeneralOptions options))
                {
                    throw new Exception($"未註冊{generalName.ToString()}參數的配置");
                }
                yield return options;
            }
        }

        public static void ActionParamsValidator(this ActionExecutingContext context)
        {
            //普通參數校驗
            context.GeneralValidator();

            //是否使用了model參數校驗
            if (!context.ExistsActionModelValidator()) return;
            //判斷當前model校驗是否通垸
            ObjectModelValidatorState objectModel = context.HttpContext.GetService<ObjectModelValidatorState>();
            if (!objectModel.Status)
            {
                context.Result = new JsonResult(objectModel);
                return;
            }

            //判斷是否提交了model參數
            if (!objectModel.HasModelContent)
            {
                context.Result = new JsonResult(new WebResponseContent().Error($"請提交參數"));
                return;
            }
        }

        /// <summary>
        /// 是否添加了ModelValidator實體校驗
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool ExistsActionModelValidator(this ActionExecutingContext context)
        {
            return context.ActionDescriptor.EndpointMetadata.Any(item => item is ObjectModelValidatorFilter);
        }
        /// <summary>
        /// 是否添加了ModelValidator實體校驗
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        public static string[] GetModelValidatorParams(this ActionContext actionContext)
        {
            return (actionContext.ActionDescriptor
                     .EndpointMetadata
                     .Where(item => item is ObjectModelValidatorFilter)
                     .FirstOrDefault() as ObjectModelValidatorFilter)?.MethodsParameters;
        }
        /// <summary>
        /// model校驗
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="prefix"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static void ModelValidator(this ActionContext actionContext, string prefix, object model)
        {
            string[] parameters = actionContext.GetModelValidatorParams();
            //沒有設置模型校驗的直接返回
            if (parameters == null) return;
            if (model == null)
            {
                actionContext.ErrorResult("沒有獲取到參數");
                return;
            }
            //model==list未判斷
            PropertyInfo[] properties = model.GetType().GetProperties().Where(x => parameters.Contains(x.Name.ToLower())).ToArray();
            foreach (var item in properties)
            {
                if (!item.ValidationRquiredValueForDbType(item.GetValue(model), out string message))
                {
                    actionContext.ErrorResult(message);
                    return;
                }
            }
            actionContext.OkModelResult();
        }

        /// <summary>
        /// 普通參數校驗
        /// </summary>
        /// <param name="actionContext"></param>
        public static void GeneralValidator(this ActionExecutingContext actionContext)
        {
            if (actionContext.ActionDescriptor
             .EndpointMetadata
             .Where(item => item is ObjectGeneralValidatorFilter)
             .FirstOrDefault() is ObjectGeneralValidatorFilter objectGeneral)
            {
                foreach (GeneralOptions general in objectGeneral.MethodsParameters)
                {
                    if (actionContext.Result != null) return;
                    if (!actionContext.HttpContext.Request.Query.TryGetValue(general.Name, out StringValues value))
                    {
                        actionContext.ActionErrorResult($"請提交參數[{general.CNName}]");
                        return;
                    }
                    if (string.IsNullOrEmpty(value))
                    {
                        actionContext.ActionErrorResult($"參數[{general.CNName}]不能為空");
                        return;
                    }
                    //自定義驗證
                    if (general.CustomValidator != null)
                    {
                        ObjectValidatorResult validatorResult = general.CustomValidator(value);
                        if (!validatorResult.Status)
                        {
                            actionContext.ActionErrorResult(validatorResult.Message);
                            return;
                        }
                        continue;
                    }
                    actionContext.ValidatorValue(general, value);
                }
            }
        }

        public static void ValidatorNumber(Type type, object value)
        {
            value.ChangeType(type);
        }
        public static void ValidatorValue(this ActionExecutingContext actionContext, GeneralOptions options, object model)
        {
            if (model == null)
            {
                actionContext.ActionErrorResult($"請提交參數{options.CNName}");
                return;
            }
            if (options.Min == null && options.Max == null) return;
            switch (options.ParamType)
            {
                //待完ParamType.Long,Byte類型
                case ParamType.Int:
                    //case ParamType.Long:
                    //case ParamType.Byte:
                    if (!int.TryParse(model.ToString(), out int _number))
                    {
                        actionContext.ActionErrorResult($"[{options.CNName}]必須是整數");
                        break;
                    }
                    if (options.Min != null && _number < options.Min)
                    {
                        actionContext.ActionErrorResult($"[{options.CNName}]不能小於[{options.Min}]");
                        break;
                    }
                    if (options.Max != null && _number > options.Max)
                    {
                        actionContext.ActionErrorResult($"[{options.CNName}]不能大於[{options.Max}]");
                    }
                    break;
                case ParamType.String:
                    string value = model.ToString();
                    if (options.Min != null && value.Length < options.Min)
                    {
                        actionContext.ActionErrorResult($"[{options.CNName}]至少[{options.Min}]個字符");
                    }
                    if (options.Max != null && value.Length > options.Max)
                    {
                        actionContext.ActionErrorResult($"[{options.CNName}]最多[{options.Max}]個字符");
                    }
                    break;
                //待完日期大小
                case ParamType.DateTime:
                    if (!DateTime.TryParse(model.ToString(),out _))
                    {
                        actionContext.ActionErrorResult($"[{options.CNName}]應該是日期格式");
                    }
                    break;
                //min,max應該是decimal類型，待完
                case ParamType.Decimal:
                    if (!decimal.TryParse(model.ToString(), out decimal _decimal))
                    {
                        actionContext.ActionErrorResult($"[{options.CNName}]不是數字");
                        break;
                    }
                    if (options.Min != null && _decimal < options.Min)
                    {
                        actionContext.ActionErrorResult($"[{options.CNName}]不能小於[{options.Min}]");
                        break;
                    }
                    if (options.Max != null && _decimal > options.Max)
                    {
                        actionContext.ActionErrorResult($"[{options.CNName}]不能大於[{options.Max}]");
                    }
                    break;
                case ParamType.Guid:
                    if (!Guid.TryParse(model.ToString(), out _))
                    {
                        actionContext.ActionErrorResult($"[{options.CNName}]不是Guid類型");
                    }
                    break;
                default:
                    break;
            }
        }

        public static void ActionErrorResult(this ActionExecutingContext actionContext, string message)
        {
            actionContext.Result = new JsonResult(new { Status = false, Message = message });
        }

        /// <summary>
        ///參數驗證沒有通過的直接返回校驗結果
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="message"></param>
        public static void ErrorResult(this ActionContext actionContext, string message)
        {
            ObjectModelValidatorState objectModel = actionContext.HttpContext.GetService<ObjectModelValidatorState>();
            if (!objectModel.Status)
            {
                return;
            }
            objectModel.Status = false;
            objectModel.Message = message;
        }
        public static void OkModelResult(this ActionContext actionContext)
        {
            ObjectModelValidatorState objectModel = actionContext.HttpContext.GetService<ObjectModelValidatorState>();
            objectModel.HasModelContent = true;
        }

    }
}
