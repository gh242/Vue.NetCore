/*
 *Author：jxx
 *Contact：283591387@qq.com
 *代碼由框架生成,此處任何更改都可能導致被代碼生成器覆蓋
 *所有業務編寫全部應在Partial文件夾下App_TransactionAvgPriceService與IApp_TransactionAvgPriceService中編寫
 */
using VOL.AppManager.IRepositories;
using VOL.AppManager.IServices;
using VOL.Core.BaseProvider;
using VOL.Core.Extensions.AutofacManager;
using VOL.Entity.DomainModels;

namespace VOL.AppManager.Services
{
    public partial class App_TransactionAvgPriceService : ServiceBase<App_TransactionAvgPrice, IApp_TransactionAvgPriceRepository>, IApp_TransactionAvgPriceService, IDependency
    {
        public App_TransactionAvgPriceService(IApp_TransactionAvgPriceRepository repository)
             : base(repository) 
        { 
           Init(repository);
        }
        public static IApp_TransactionAvgPriceService Instance
        {
           get { return AutofacContainerModule.GetService<IApp_TransactionAvgPriceService>(); }
        }
    }
}
