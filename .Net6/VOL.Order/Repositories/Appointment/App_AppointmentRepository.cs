/*
 *代碼由框架生成,任何更改都可能導致被代碼生成器覆蓋
 *Repository提供數據庫操作，如果要增加數據庫操作請在當前目錄下Partial文件夾App_AppointmentRepository編寫代碼
 */
using VOL.Order.IRepositories;
using VOL.Core.BaseProvider;
using VOL.Core.EFDbContext;
using VOL.Core.Extensions.AutofacManager;
using VOL.Entity.DomainModels;

namespace VOL.Order.Repositories
{
    public partial class App_AppointmentRepository : RepositoryBase<App_Appointment> , IApp_AppointmentRepository
    {
    public App_AppointmentRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static IApp_AppointmentRepository Instance
    {
      get {  return AutofacContainerModule.GetService<IApp_AppointmentRepository>(); } }
    }
}
