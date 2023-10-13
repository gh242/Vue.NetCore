/*
 *接口編寫處...
*如果接口需要做Action的權限驗證，請在Action上使用屬性
*如: [ApiActionPermission("Demo_Customer",Enums.ActionPermissionOptions.Search)]
 */
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using VOL.Entity.DomainModels;
using VOL.Demo.IServices;
using VOL.Core.Filters;

namespace VOL.Demo.Controllers
{
    public partial class Demo_CustomerController
    {
        private readonly IDemo_CustomerService _service;//訪問業務代碼
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Demo_CustomerController(
            IDemo_CustomerService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }

        /************重寫權限重写权限************/

        /// <summary>
        /// 页面数据查询
        /// </summary>
        /// <param name="loadData"></param>
        /// <returns></returns>
        /// ApiActionPermission中的内容清空後，只會驗證用戶是否登入，不會驗證用戶查詢權限
        //[ApiActionPermission()]
        //第一個参數可以輸入表名，指定某張表的權限
        //[ApiActionPermission("SellOrder",ActionPermissionOptions.Search)]
        [HttpPost, Route("GetPageData")]
        [ApiActionPermission()]
        public override ActionResult GetPageData([FromBody] PageDataOptions loadData)
        {
            return base.GetPageData(loadData);
        }
    }
}
