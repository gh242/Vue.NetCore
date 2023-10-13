/*
 *代碼由框架生成,任何更改都可能導致被代碼生成器覆蓋
 *如果要增加方法請在當前目錄下Partial文件夾App_AppointmentController編寫
 */
using Microsoft.AspNetCore.Mvc;
using VOL.Core.Controllers.Basic;
using VOL.Entity.AttributeManager;
using VOL.Order.IServices;
namespace VOL.Order.Controllers
{
    [Route("api/App_Appointment")]
    [PermissionTable(Name = "App_Appointment")]
    public partial class App_AppointmentController : ApiBaseController<IApp_AppointmentService>
    {
        public App_AppointmentController(IApp_AppointmentService service)
        : base("Order","Appointment","App_Appointment", service)
        {
        }
    }
}

