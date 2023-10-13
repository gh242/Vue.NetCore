/*
 *代碼由框架生成,此處任何更改都可能導致被代碼生成器覆蓋
 *所有業務編寫全部應在Partial文件夾下test2019Service與Itest2019Service中編寫
 */
using VOL.AppManager.IRepositories;
using VOL.AppManager.IServices;
using VOL.Core.BaseProvider;
using VOL.Core.Extensions.AutofacManager;
using VOL.Entity.DomainModels;

namespace VOL.AppManager.Services
{
    public partial class test2019Service : ServiceBase<test2019, Itest2019Repository>, Itest2019Service, IDependency
    {
        public test2019Service(Itest2019Repository repository)
             : base(repository) 
        { 
           Init(repository);
        }
        public static Itest2019Service Instance
        {
           get { return AutofacContainerModule.GetService<Itest2019Service>(); }
        }
    }
}
