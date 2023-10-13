/*
 *Author：jxx
 *Contact：283591387@qq.com
 *代碼由框架生成,此處任何更改都可能導致被代碼生成器覆蓋
 *所有業務編寫全部應在Partial文件夾下Demo_GoodsService與IDemo_GoodsService中編寫
 */
using VOL.Demo.IRepositories;
using VOL.Demo.IServices;
using VOL.Core.BaseProvider;
using VOL.Core.Extensions.AutofacManager;
using VOL.Entity.DomainModels;

namespace VOL.Demo.Services
{
    public partial class Demo_GoodsService : ServiceBase<Demo_Goods, IDemo_GoodsRepository>
    , IDemo_GoodsService, IDependency
    {
    public Demo_GoodsService(IDemo_GoodsRepository repository)
    : base(repository)
    {
    Init(repository);
    }
    public static IDemo_GoodsService Instance
    {
      get { return AutofacContainerModule.GetService<IDemo_GoodsService>(); } }
    }
 }
