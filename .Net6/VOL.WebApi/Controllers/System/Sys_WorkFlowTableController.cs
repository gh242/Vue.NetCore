/*
 *代碼由框架生成,任何更改都可能導致被代碼生成器覆蓋
 *如果要增加方法請在當前目錄下Partial文件夾Sys_WorkFlowTableController編寫
 */
using Microsoft.AspNetCore.Mvc;
using VOL.Core.Controllers.Basic;
using VOL.Entity.AttributeManager;
using VOL.System.IServices;
namespace VOL.System.Controllers
{
    [Route("api/Sys_WorkFlowTable")]
    [PermissionTable(Name = "Sys_WorkFlowTable")]
    public partial class Sys_WorkFlowTableController : ApiBaseController<ISys_WorkFlowTableService>
    {
        public Sys_WorkFlowTableController(ISys_WorkFlowTableService service)
        : base(service)
        {
        }
    }
}

