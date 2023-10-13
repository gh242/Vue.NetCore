/*
 *代碼由框架生成,任何更改都可能導致被代碼生成器覆蓋
 *如果要增加方法請在當前目錄下Partial文件夾Sys_WorkFlowController編寫
 */
using Microsoft.AspNetCore.Mvc;
using VOL.Core.Controllers.Basic;
using VOL.Entity.AttributeManager;
using VOL.System.IServices;
namespace VOL.System.Controllers
{
    [Route("api/Sys_WorkFlow")]
    [PermissionTable(Name = "Sys_WorkFlow")]
    public partial class Sys_WorkFlowController : ApiBaseController<ISys_WorkFlowService>
    {
        public Sys_WorkFlowController(ISys_WorkFlowService service)
        : base(service)
        {
        }
    }
}

