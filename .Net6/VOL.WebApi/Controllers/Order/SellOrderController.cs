/*
 *代碼由框架生成,任何更改都可能導致被代碼生成器覆蓋
 *如果要增加方法請在當前目錄下Partial文件夾SellOrderController編寫
 */
using Microsoft.AspNetCore.Mvc;
using VOL.Core.Controllers.Basic;
using VOL.Entity.AttributeManager;
using VOL.Order.IServices;
namespace VOL.Order.Controllers
{
    [Route("api/SellOrder")]
    [PermissionTable(Name = "SellOrder")]
    public partial class SellOrderController : ApiBaseController<ISellOrderService>
    {
        public SellOrderController(ISellOrderService service)
        : base(service)
        {
        }
    }
}

