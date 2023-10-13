using System;
using System.Collections.Generic;
using System.Text;

namespace VOL.Core.Enums
{
    public struct ApiMessage
    {

        /// <summary>
        /// 參數有誤
        /// </summary>
        public const string ParameterError = "請求參數不正確!";
        /// <summary>
        /// 沒有配置好輸入參數
        /// </summary>
        public const string NotInputEntity = "沒有配置好輸入參數!";
        /// <summary>
        /// token丢失
        /// </summary>
        public const string TokenLose = "token丢失!";

        /// <summary>
        /// 版本號不能為空
        /// </summary>

        public const string VersionEmpty = "版本號不能為空!";
        /// <summary>
        /// content不能為空
        /// </summary>

        public const string ContentEmpty = "biz_content不能為空!";
        /// <summary>
        /// content不能為空
        /// </summary>
        public const string TokenError = "token不正確";

        public const string AccountLocked = "帳號已被鎖定!";

        public const string PhoneNoInvalid = "輸入的不是手機號";


        public const string PINTypeNotRange= "獲取驗證的類型不正確";
        public const string OperToBusy = "操作太頻繁，請稍後再試";

        public const string SendSTKError = "短信發送異常,請稍後再試";
        public const string SendSTKSuccess = "短信發送成功";
        public const string STKNotSend = "請先獲取驗證碼";
        public const string AccountExists = "手機號已經被註冊";

        public const string AccountNotExists = "手機號沒有註冊";

        public const string PINExpire = "驗證碼已過期,請重新獲取";

        public const string PINError = "驗證碼不正確";

        public const string ParameterEmpty = "參數不能為空";
    }
}
