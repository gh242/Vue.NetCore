/*
 *Author：jxx
 *Contact：283591387@qq.com
 *代碼由框架生成,此處任何更改都可能導致被代碼生成器覆蓋
 *所有業務編寫全部應在Partial文件夾下Demo_CatalogService與IDemo_CatalogService中編寫
 */
using VOL.Demo.IRepositories;
using VOL.Demo.IServices;
using VOL.Core.BaseProvider;
using VOL.Core.Extensions.AutofacManager;
using VOL.Entity.DomainModels;

namespace VOL.Demo.Services
{
    public partial class Demo_CatalogService : ServiceBase<Demo_Catalog, IDemo_CatalogRepository>
    , IDemo_CatalogService, IDependency
    {
    public Demo_CatalogService(IDemo_CatalogRepository repository)
    : base(repository)
    {
    Init(repository);
    }
    public static IDemo_CatalogService Instance
    {
      get { return AutofacContainerModule.GetService<IDemo_CatalogService>(); } }
    }
 }
