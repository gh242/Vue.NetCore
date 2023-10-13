using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace VOL.Core.Services
{
   public class ActionObserver
    {
        //public ActionObserver(IHttpContextAccessor httpContextAccessor)
        //{
        //    this.RequestDate = DateTime.Now;
        //    this.HttpContext = httpContextAccessor.HttpContext;
        //}
        /// <summary>
        /// 記錄action執行的開始時間
        /// </summary>
        public DateTime RequestDate { get; set; }

        /// <summary>
        /// 當前請求是否已經寫過日誌,防止手動與系統自動重覆寫日誌
        /// </summary>
        public bool IsWrite { get; set; }

        public HttpContext HttpContext { get; }
    }
}
