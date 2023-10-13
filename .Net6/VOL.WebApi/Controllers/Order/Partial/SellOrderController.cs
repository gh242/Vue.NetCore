/*
 *接口編寫處...
*如果接口需要做Action的權限驗證，請在Action上使用屬性
*如: [ApiActionPermission("SellOrder",Enums.ActionPermissionOptions.Search)]
 */
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using VOL.Core.CacheManager;
using VOL.Core.Enums;
using VOL.Core.Filters;
using VOL.Core.ManageUser;
using VOL.Entity.DomainModels;
using VOL.Order.IRepositories;
using VOL.Order.IServices;
using VOL.Order.Repositories;
using VOL.WebApi.Controllers.Hubs;

namespace VOL.Order.Controllers
{
    public partial class SellOrderController
    {
        private readonly ICacheService _cacheService;
        private readonly IHubContext<HomePageMessageHub> _hubContext;

        private readonly ISellOrderRepository _orderRepository;
        [ActivatorUtilitiesConstructor]

        public SellOrderController(ISellOrderRepository orderRepository, IHubContext<HomePageMessageHub> hubContext, ICacheService cacheService, ISellOrderService service) : base(service)
        {
            //數據庫訪問，更多操作見後台開發：數據庫訪問
            _orderRepository = orderRepository;
            _hubContext = hubContext;
            _cacheService = cacheService;
            //http://localhost:8081/document/netCoreDev
        }

        [HttpPost]
        [ApiActionPermission("SellOrder", Core.Enums.ActionPermissionOptions.Search)]
        [Route("getServiceDate"), FixedToken]//FixedToken請求此接口只要token合法就能能過//AllowAnonymous
        public IActionResult GetServiceDate()
        {
            return Content(Service.GetServiceDate());
        }

        /************重寫權限************/

        /// <summary>
        /// 頁面數據查詢
        /// </summary>
        /// <param name="loadData"></param>
        /// <returns></returns>
        /// ApiActionPermission注釋後，只會驗證用戶是否登入，不會驗證用戶查詢權限
        //[ApiActionPermission(ActionPermissionOptions.Search)]
        //第一個參數可以輸入表名，指定某張表的權限
        [ApiActionPermission()]
        [HttpPost, Route("GetPageData"), AllowAnonymous]
        public override ActionResult GetPageData([FromBody] PageDataOptions loadData)
        {
            return base.GetPageData(loadData);
            //var list = Service.GetPageData(loadData).rows;
            ////獲取ConnectionId
            //var key = "SignalR" + UserContext.Current.UserId;
            //var cid = _cacheService.Get(key);
            ////測試SignalR推送業務數據
            //if (!string.IsNullOrEmpty(cid))
            //    _hubContext.Clients.Client(cid).SendAsync("SellOrderData", list);

            //return base.GetPageData(loadData);
        }

        /// <summary>
        /// 新增操作（權限重寫同上）
        /// </summary>
        /// <param name="saveModel"></param>
        /// <returns></returns>
        //[ApiActionPermission("SellOrder", ActionPermissionOptions.Add)]
        [HttpPost, Route("Add")]
        public override ActionResult Add([FromBody] SaveModel saveModel)
        {
            return base.Add(saveModel);
        }
        /// <summary>
        ///編譯操作（權限重寫同上）
        /// </summary>
        /// <param name="saveModel"></param>
        /// <returns></returns>
       // [ApiActionPermission(ActionPermissionOptions.Update)]
        [HttpPost, Route("Update")]
        public override ActionResult Update([FromBody] SaveModel saveModel)
        {
            return base.Update(saveModel);
        }
        /// <summary>
        /// 通過key刪除文件（權限重寫同上）
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        //  [ApiActionPermission(ActionPermissionOptions.Delete)]
        [HttpPost, Route("Del")]
        public override ActionResult Del([FromBody] object[] keys)
        {
            return base.Del(keys);
        }

        ///更多可重寫的權限見ApiBaseController

    }
}
