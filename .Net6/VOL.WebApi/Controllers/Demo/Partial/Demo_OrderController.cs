/*
 *接口編寫處...
*如果接口需要做Action的權限驗證，請在Action上使用屬性
*如: [ApiActionPermission("Demo_Order",Enums.ActionPermissionOptions.Search)]
 */
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using VOL.Entity.DomainModels;
using VOL.Demo.IServices;
using Microsoft.AspNetCore.Authorization;

namespace VOL.Demo.Controllers
{
    public partial class Demo_OrderController
    {
        private readonly IDemo_OrderService _service;//訪問業務代碼
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDemo_GoodsService _goodService;

        [ActivatorUtilitiesConstructor]
        public Demo_OrderController(
            IDemo_OrderService service,
            IHttpContextAccessor httpContextAccessor,
            IDemo_GoodsService goodService
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
            _goodService = goodService;
        }



        public override ActionResult GetPageData([FromBody] PageDataOptions loadData)
        {
            return base.GetPageData(loadData);
        }


        [HttpGet, Route("test"), AllowAnonymous]
        public IActionResult Test()
        {
            return Content("OK");
        }        
        
        [HttpGet, Route("test1")]
        public IActionResult Test1()
        {
            return Content("test1");
        }


        //批量選擇獲取商品數據
        [Route("getGoods"), HttpPost]
        public IActionResult GetGoods([FromBody] PageDataOptions loadData)
        {
            //return base.GetPageData(loadData);
            var gridData = _goodService.GetPageData(loadData);
            return JsonNormal(gridData);  // 回傳的值保持大小寫
        }
    }
}
