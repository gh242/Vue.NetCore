/*
 *Author：jxx
 *Contact：283591387@qq.com
 *代碼由框架生成,此處任何更改都可能導致被代碼生成器覆蓋
 *所有業務編寫全部應在Partial文件夾下App_AppointmentService與IApp_AppointmentService中編寫
 */
using VOL.Order.IRepositories;
using VOL.Order.IServices;
using VOL.Core.BaseProvider;
using VOL.Core.Extensions.AutofacManager;
using VOL.Entity.DomainModels;

namespace VOL.Order.Services
{
    public partial class App_AppointmentService : ServiceBase<App_Appointment, IApp_AppointmentRepository>, IApp_AppointmentService, IDependency
    {
        public App_AppointmentService(IApp_AppointmentRepository repository)
             : base(repository) 
        { 
           Init(repository);
        }
        public static IApp_AppointmentService Instance
        {
           get { return AutofacContainerModule.GetService<IApp_AppointmentService>(); }
        }
    }
}
