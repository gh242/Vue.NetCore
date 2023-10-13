/*
 *Author：jxx
 *Contact：283591387@qq.com
 *代碼由框架生成,此處任何更改都可能導致被代碼生成器覆蓋
 *所有業務編寫全部應在Partial文件夾下Demo_OrderListService與IDemo_OrderListService中編寫
 */
using VOL.Demo.IRepositories;
using VOL.Demo.IServices;
using VOL.Core.BaseProvider;
using VOL.Core.Extensions.AutofacManager;
using VOL.Entity.DomainModels;

namespace VOL.Demo.Services
{
    public partial class Demo_OrderListService : ServiceBase<Demo_OrderList, IDemo_OrderListRepository>
    , IDemo_OrderListService, IDependency
    {
    public Demo_OrderListService(IDemo_OrderListRepository repository)
    : base(repository)
    {
    Init(repository);
    }
    public static IDemo_OrderListService Instance
    {
      get { return AutofacContainerModule.GetService<IDemo_OrderListService>(); } }
    }
 }
