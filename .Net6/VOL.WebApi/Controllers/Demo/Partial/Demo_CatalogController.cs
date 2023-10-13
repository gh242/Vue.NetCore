/*
 *接口編寫處...
*如果接口需要做Action的權限驗證，請在Action上使用屬性
*如: [ApiActionPermission("Demo_Catalog",Enums.ActionPermissionOptions.Search)]
 */
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using VOL.Entity.DomainModels;
using VOL.Demo.IServices;
using VOL.Demo.IRepositories;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace VOL.Demo.Controllers
{
    public partial class Demo_CatalogController
    {
        private readonly IDemo_CatalogService _service;//訪問業務代碼
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDemo_CatalogService _orderService;
        private readonly IDemo_OrderRepository _orderRepository;

        [ActivatorUtilitiesConstructor]
        public Demo_CatalogController(
            IDemo_CatalogService service,
            IHttpContextAccessor httpContextAccessor,
            IDemo_CatalogService orderService,
            IDemo_OrderRepository orderRepository
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
            _orderService = orderService;
            _orderRepository = orderRepository;
        }

        public override ActionResult GetPageData([FromBody] PageDataOptions loadData)
        {
            //_orderService.
            var data = _orderRepository.FindAsIQueryable(x => x.OrderNo == "xxx").Select(s => new
            {
                s.OrderNo,
                s.OrderStatus
            //}).ToListAsync();
            }).ToList();
            return base.GetPageData(loadData);
        }
    }
}
