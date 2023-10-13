/*
 *代碼由框架生成,任何更改都可能導致被代碼生成器覆蓋
 *如果要增加方法請在當前目錄下Partial文件夾App_ExpertController編寫
 */
using Microsoft.AspNetCore.Mvc;
using VOL.AppManager.IServices;
using VOL.Core.Controllers.Basic;
using VOL.Entity.AttributeManager;

namespace VOL.AppManager.Controllers
{
    [Route("api/App_Expert")]
    [PermissionTable(Name = "App_Expert")]
    public partial class App_ExpertController : ApiBaseController<IApp_ExpertService>
    {
        public App_ExpertController(IApp_ExpertService service)
        : base("AppManager","App","App_Expert", service)
        {
        }
    }
}

