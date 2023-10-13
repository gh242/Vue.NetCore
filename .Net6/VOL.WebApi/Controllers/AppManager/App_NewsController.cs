/*
 *代碼由框架生成,任何更改都可能導致被代碼生成器覆蓋
 *如果要增加方法請在當前目錄下Partial文件夾App_NewsController編寫
 */
using Microsoft.AspNetCore.Mvc;
using VOL.Core.Controllers.Basic;
using VOL.Entity.AttributeManager;
using VOL.AppManager.IServices;
namespace VOL.AppManager.Controllers
{
    [Route("api/App_News")]
    [PermissionTable(Name = "App_News")]
    public partial class App_NewsController : ApiBaseController<IApp_NewsService>
    {
        public App_NewsController(IApp_NewsService service)
        : base(service)
        {
        }
    }
}

