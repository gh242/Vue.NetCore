using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using VOL.Core.Extensions;
using VOL.Entity.DomainModels;

namespace VOL.Core.ObjectActionValidator
{
    /// <summary>
    /// 對方法指定屬性校驗,此處配置完成就不用每處都寫if esle判斷值是合法
    /// 與自帶模型校驗相比，此處可以通過表達式校驗指定字段，也不用担心model字段變化後還去手動修改配置的問題
    /// 目前只支持普通屬性，不支持複雜類型
    /// </summary>
    public static class ValidatorContainer
    {
        /// <summary>
        /// model校驗配置
        /// 方法參數名必須與枚舉名稱一致（不區分大小寫）,如：public void Test(LoginInfo login)
        /// 表達式是model必須要驗證的字段，如果不填，默認驗證整個model
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection UseMethodsModelParameters(this IServiceCollection services)
        {
            //登入方法校驗參數,只驗證密碼與用戶名
            ValidatorModel.Login.Add<LoginInfo>(x => new { x.Password, x.UserName,x.VerificationCode,x.UUID });

            //只驗證LoginInfo的密碼字段必填
            ValidatorModel.LoginOnlyPassWord.Add<LoginInfo>(x => new { x.Password });

            return services;
        }
        /// <summary>
        ///  普通屬性校驗
        /// 方法上添加[ObjectGeneralValidatorFilter(ValidatorGeneral.xxx)]即可進行參數自動驗證
        /// ValidatorGeneral為枚舉(也是方法的參數名)，自己需要校驗的參數在枚舉上添加
        /// ValidatorGeneral.xxx.Add() 配置自己的驗證規則
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection UseMethodsGeneralParameters(this IServiceCollection services)
        {
            //配置用戶名最多30個字符
            ValidatorGeneral.UserName.Add("用戶名", 30);

            //方法參數名為newPwd，直接在方法加上[ObjectGeneralValidatorFilter(ValidatorGeneral.NewPwd)]進行參數驗證
            //如果newPwd為空會提示：新密碼不能為空
            //6,50代表newPwd參數最少6個字符，最多50個符
            //其他需要驗證的參數同樣配置即可
            ValidatorGeneral.NewPwd.Add("新密碼", 6, 50);

            //如果OldPwd為空會提示：舊密碼不能為空
            ValidatorGeneral.OldPwd.Add("舊密碼");

            //校驗手機號碼格式
            ValidatorGeneral.PhoneNo.Add("手機號碼", (object value) =>
            {
                ObjectValidatorResult validatorResult = new ObjectValidatorResult(true);
                if (!value.ToString().IsPhoneNo())
                {
                    validatorResult = validatorResult.Error("請輸入正確的手機號碼");
                }
                return validatorResult;
            });

            //測試驗證字符長度為6-10
            ValidatorGeneral.Local.Add("所在地",6,10);

            //測試驗證數字範圍
            ValidatorGeneral.Qty.Add("存貨量",ParamType.Int, 200, 500);

            return services;
        }
    }
    //方法參數是實體配置驗證字段
    public enum ValidatorModel
    {
        Login,
        LoginOnlyPassWord//只驗證密碼
    }
    /// <summary>
    /// 方法普通參數名配置(枚舉的名字必須與參數名字一樣，不區分大小寫)
    /// 通過在方法加上[ObjectGeneralValidatorFilter(ValidatorGeneral.UserName, ValidatorGeneral.PassWord)]指定要驗證的參數
    /// </summary>
    public enum ValidatorGeneral
    {
        UserName,
        OldPwd,
        NewPwd,
        PhoneNo,
        Local,//測試驗證字符長度
        Qty//測試 驗證值大小
    }
}
