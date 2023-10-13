/*
 *代碼由框架生成,任何更改都可能導致被代碼生成器覆蓋
 *如果要增加方法請在當前目錄下Partial文件夾Demo_OrderController編寫
 */
using Microsoft.AspNetCore.Mvc;
using VOL.Core.Controllers.Basic;
using VOL.Entity.AttributeManager;
using VOL.Demo.IServices;
namespace VOL.Demo.Controllers
{
    [Route("api/Demo_Order")]
    [PermissionTable(Name = "Demo_Order")]
    public partial class Demo_OrderController : ApiBaseController<IDemo_OrderService>
    {
        public Demo_OrderController(IDemo_OrderService service)
        : base(service)
        {
        }
    }
}

