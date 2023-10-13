/*
 *代碼由框架生成,任何更改都可能導致被代碼生成器覆蓋
 *如果要增加方法請在當前目錄下Partial文件夾View_OrderInfoController編寫
 */
using Microsoft.AspNetCore.Mvc;
using VOL.Core.Controllers.Basic;
using VOL.Entity.AttributeManager;
using VOL.Demo.IServices;
namespace VOL.Demo.Controllers
{
    [Route("api/View_OrderInfo")]
    [PermissionTable(Name = "View_OrderInfo")]
    public partial class View_OrderInfoController : ApiBaseController<IView_OrderInfoService>
    {
        public View_OrderInfoController(IView_OrderInfoService service)
        : base(service)
        {
        }
    }
}

