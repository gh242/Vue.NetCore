/*
 *Author：jxx
 *Contact：283591387@qq.com
 *代碼由框架生成,此處任何更改都可能導致被代碼生成器覆蓋
 *所有業務編寫全部應在Partial文件夾下App_ReportPriceService與IApp_ReportPriceService中編寫
 */
using VOL.AppManager.IRepositories;
using VOL.AppManager.IServices;
using VOL.Core.BaseProvider;
using VOL.Core.Extensions.AutofacManager;
using VOL.Entity.DomainModels;

namespace VOL.AppManager.Services
{
    public partial class App_ReportPriceService : ServiceBase<App_ReportPrice, IApp_ReportPriceRepository>, IApp_ReportPriceService, IDependency
    {
        public App_ReportPriceService(IApp_ReportPriceRepository repository)
             : base(repository) 
        { 
           Init(repository);
        }
        public static IApp_ReportPriceService Instance
        {
           get { return AutofacContainerModule.GetService<IApp_ReportPriceService>(); }
        }
    }
}
