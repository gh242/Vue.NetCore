/*
 *代碼由框架生成,任何更改都可能導致被代碼生成器覆蓋
 *如果要增加方法請在當前目錄下Partial文件夾Demo_OrderListController編寫
 */
using Microsoft.AspNetCore.Mvc;
using VOL.Core.Controllers.Basic;
using VOL.Entity.AttributeManager;
using VOL.Demo.IServices;
namespace VOL.Demo.Controllers
{
    [Route("api/Demo_OrderList")]
    [PermissionTable(Name = "Demo_OrderList")]
    public partial class Demo_OrderListController : ApiBaseController<IDemo_OrderListService>
    {
        public Demo_OrderListController(IDemo_OrderListService service)
        : base(service)
        {
        }
    }
}

