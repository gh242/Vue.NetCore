/*
 *Author：jxx
 *Contact：283591387@qq.com
 *代碼由框架生成,此處任何更改都可能導致被代碼生成器覆蓋
 *所有業務編寫全部應在Partial文件夾下Sys_DepartmentService與ISys_DepartmentService中編寫
 */
using VOL.System.IRepositories;
using VOL.System.IServices;
using VOL.Core.BaseProvider;
using VOL.Core.Extensions.AutofacManager;
using VOL.Entity.DomainModels;

namespace VOL.System.Services
{
    public partial class Sys_DepartmentService : ServiceBase<Sys_Department, ISys_DepartmentRepository>
    , ISys_DepartmentService, IDependency
    {
    public Sys_DepartmentService(ISys_DepartmentRepository repository)
    : base(repository)
    {
    Init(repository);
    }
    public static ISys_DepartmentService Instance
    {
      get { return AutofacContainerModule.GetService<ISys_DepartmentService>(); } }
    }
 }
