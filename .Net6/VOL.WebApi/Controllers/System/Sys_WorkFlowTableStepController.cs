/*
 *代碼由框架生成,任何更改都可能導致被代碼生成器覆蓋
 *如果要增加方法請在當前目錄下Partial文件夾Sys_WorkFlowTableStepController編寫
 */
using Microsoft.AspNetCore.Mvc;
using VOL.Core.Controllers.Basic;
using VOL.Entity.AttributeManager;
using VOL.System.IServices;
namespace VOL.System.Controllers
{
    [Route("api/Sys_WorkFlowTableStep")]
    [PermissionTable(Name = "Sys_WorkFlowTableStep")]
    public partial class Sys_WorkFlowTableStepController : ApiBaseController<ISys_WorkFlowTableStepService>
    {
        public Sys_WorkFlowTableStepController(ISys_WorkFlowTableStepService service)
        : base(service)
        {
        }
    }
}

