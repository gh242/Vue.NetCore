/*
 *Author：jxx
 *Contact：283591387@qq.com
 *代碼由框架生成,此處任何更改都可能導致被代碼生成器覆蓋
 *所有業務編寫全部應在Partial文件夾下App_TransactionService與IApp_TransactionService中編寫
 */
using VOL.AppManager.IRepositories;
using VOL.AppManager.IServices;
using VOL.Core.BaseProvider;
using VOL.Core.Extensions.AutofacManager;
using VOL.Entity.DomainModels;

namespace VOL.AppManager.Services
{
    public partial class App_TransactionService : ServiceBase<App_Transaction, IApp_TransactionRepository>, IApp_TransactionService, IDependency
    {
        public App_TransactionService(IApp_TransactionRepository repository)
             : base(repository) 
        { 
           Init(repository);
        }
        public static IApp_TransactionService Instance
        {
           get { return AutofacContainerModule.GetService<IApp_TransactionService>(); }
        }
    }
}
