using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VOL.Core.Filters;
using VOL.Core.ObjectActionValidator;
using VOL.Entity.DomainModels;

namespace VOL.WebApi.Controllers
{
    /// <summary>
    ///1、普通參數校驗只需要標識屬性：[ObjectGeneralValidatorFilter(ValidatorGeneral.xxx,ValidatorGeneral.xxx)]，
    ///需要在ValidatorGeneral枚舉中添加枚舉值(參數名)，並在UseMethodsGeneralParameters方法中注入進去即可在任何地方重覆使用
    /// 
    /// 2、model校驗只需要標識屬性[ObjectModelValidatorFilter(ValidatorModel.xxx)]
    /// 需要在ValidatorModel枚舉中添加枚舉值(參數名)，
    /// 並在UseMethodsModelParameters方法中注入進去(注入時可以指定需要驗證的字段)即可在任何地方重覆使用
    /// --如果其他方法使用的是同一個model，但驗證的字段不同，在ValidatorModel重新添加一個枚舉值，
    /// --並在UseMethodsModelParameters方法注入,添加新的指定字段即可
    /// </summary>
    [JWTAuthorize, ApiController]
    [Route("validatorExample")]
    public class ObjectActionValidatorExampleController: Controller
    {
        public ObjectActionValidatorExampleController()
        { 
        
        }

        /// <summary>
        /// 驗證UserName與PhoneNo為必填
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="phoneNo"></param>
        /// <returns></returns>
        [HttpPost, HttpGet, Route("test1")]
        [ObjectGeneralValidatorFilter(ValidatorGeneral.UserName,ValidatorGeneral.PhoneNo)]
        public IActionResult Test1(string userName,string phoneNo)
        {
            return Json("參數驗證通過");
        }

        /// <summary>
        /// 驗證PhoneNo為必填
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="phoneNo"></param>
        /// <returns></returns>
        [HttpPost, HttpGet, Route("test2")]
        [ObjectGeneralValidatorFilter(ValidatorGeneral.PhoneNo)]
        public IActionResult Test2(string userName, string phoneNo)
        {
            return Json("參數驗證通過");
        }


        /// <summary>
        /// 驗證字符長度與值大小
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="phoneNo"></param>
        /// <returns></returns>
        [HttpPost, HttpGet, Route("test3")]
        [ObjectGeneralValidatorFilter(ValidatorGeneral.Local, ValidatorGeneral.Qty)]
        public IActionResult Test3(string local, string qty)
        {
            return Json("參數驗證通過");
        }

        /// <summary>
        /// Login配置的規則用戶名與密碼必填
        /// </summary>
        /// <param name="loginInfo"></param>
        /// <returns></returns>
        [HttpPost, HttpGet, Route("Test4")]
        [ObjectModelValidatorFilter(ValidatorModel.Login)]
        public IActionResult Test4([FromBody]LoginInfo loginInfo)
        {
            return Json("參數驗證通過");
        }

        /// <summary>
        /// LoginOnlyPassWord配置的規則密碼必填
        /// </summary>
        /// <param name="loginInfo"></param>
        /// <returns></returns>
        [HttpPost, HttpGet, Route("Test5")]
        [ObjectModelValidatorFilter(ValidatorModel.LoginOnlyPassWord)]
        public IActionResult Test5([FromBody]LoginInfo loginInfo)
        {
            return Json("參數驗證通過");
        }


        /// <summary>
        /// 同時驗證實體LoginInfo與單個參數phoneNo
        ///  Login配置的規則用戶名與密碼必填,手機號必填
        /// </summary>
        /// <param name="loginInfo"></param>
        /// <returns></returns>
        [HttpPost, HttpGet, Route("Test6")]
        [ObjectModelValidatorFilter(ValidatorModel.Login)]
        [ObjectGeneralValidatorFilter(ValidatorGeneral.PhoneNo)]
        public IActionResult Test6([FromBody]LoginInfo loginInfo, string phoneNo)
        {
            return Json("參數驗證通過");
        }
    }
}
