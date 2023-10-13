/*
 *Author：jxx
 *Contact：283591387@qq.com
 *代碼由框架生成,此處任何更改都可能導致被代碼生成器覆蓋
 *所有業務編寫全部應在Partial文件夾下Demo_CustomerService與IDemo_CustomerService中編寫
 */
using VOL.Demo.IRepositories;
using VOL.Demo.IServices;
using VOL.Core.BaseProvider;
using VOL.Core.Extensions.AutofacManager;
using VOL.Entity.DomainModels;

namespace VOL.Demo.Services
{
    public partial class Demo_CustomerService : ServiceBase<Demo_Customer, IDemo_CustomerRepository>
    , IDemo_CustomerService, IDependency
    {
    public Demo_CustomerService(IDemo_CustomerRepository repository)
    : base(repository)
    {
    Init(repository);
    }
    public static IDemo_CustomerService Instance
    {
      get { return AutofacContainerModule.GetService<IDemo_CustomerService>(); } }
    }
 }
