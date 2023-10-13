/*
 *Author：jxx
 *Contact：283591387@qq.com
 *代碼由框架生成,此處任何更改都可能導致被代碼生成器覆蓋
 *所有業務編寫全部應在Partial文件夾下View_OrderInfoService與IView_OrderInfoService中編寫
 */
using VOL.Demo.IRepositories;
using VOL.Demo.IServices;
using VOL.Core.BaseProvider;
using VOL.Core.Extensions.AutofacManager;
using VOL.Entity.DomainModels;

namespace VOL.Demo.Services
{
    public partial class View_OrderInfoService : ServiceBase<View_OrderInfo, IView_OrderInfoRepository>
    , IView_OrderInfoService, IDependency
    {
    public View_OrderInfoService(IView_OrderInfoRepository repository)
    : base(repository)
    {
    Init(repository);
    }
    public static IView_OrderInfoService Instance
    {
      get { return AutofacContainerModule.GetService<IView_OrderInfoService>(); } }
    }
 }
