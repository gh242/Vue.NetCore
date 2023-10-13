/*
 *Author：jxx
 *Contact：283591387@qq.com
 *代碼由框架生成,此處任何更改都可能導致被代碼生成器覆蓋
 *所有業務編寫全部應在Partial文件夾下App_NewsService與IApp_NewsService中編寫
 */
using VOL.AppManager.IRepositories;
using VOL.AppManager.IServices;
using VOL.Core.BaseProvider;
using VOL.Core.Extensions.AutofacManager;
using VOL.Entity.DomainModels;

namespace VOL.AppManager.Services
{
    public partial class App_NewsService : ServiceBase<App_News, IApp_NewsRepository>, IApp_NewsService, IDependency
    {
        public App_NewsService(IApp_NewsRepository repository)
             : base(repository) 
        { 
           Init(repository);
        }
        public static IApp_NewsService Instance
        {
           get { return AutofacContainerModule.GetService<IApp_NewsService>(); }
        }
    }
}
