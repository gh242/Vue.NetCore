/*
 *代碼由框架生成,任何更改都可能導致被代碼生成器覆蓋
 *如果要增加方法請在當前目錄下Partial文件夾Sys_QuartzLogController編寫
 */
using Microsoft.AspNetCore.Mvc;
using VOL.Core.Controllers.Basic;
using VOL.Entity.AttributeManager;
using VOL.System.IServices;
namespace VOL.System.Controllers
{
    [Route("api/Sys_QuartzLog")]
    [PermissionTable(Name = "Sys_QuartzLog")]
    public partial class Sys_QuartzLogController : ApiBaseController<ISys_QuartzLogService>
    {
        public Sys_QuartzLogController(ISys_QuartzLogService service)
        : base(service)
        {
        }
    }
}

