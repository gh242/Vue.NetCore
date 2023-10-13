/*
 *代碼由框架生成,任何更改都可能導致被代碼生成器覆蓋
 *如果要增加方法請在當前目錄下Partial文件夾App_ReportPriceController編寫
 */
using Microsoft.AspNetCore.Mvc;
using VOL.Core.Controllers.Basic;
using VOL.Entity.AttributeManager;
using VOL.AppManager.IServices;
namespace VOL.AppManager.Controllers
{
    [Route("api/App_ReportPrice")]
    [PermissionTable(Name = "App_ReportPrice")]
    public partial class App_ReportPriceController : ApiBaseController<IApp_ReportPriceService>
    {
        public App_ReportPriceController(IApp_ReportPriceService service)
        : base(service)
        {
        }
    }
}

