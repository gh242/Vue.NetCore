/*
 *Author：jxx
 *Contact：283591387@qq.com
 *代碼由框架生成,此處任何更改都可能導致被代碼生成器覆蓋
 *所有業務編寫全部應在Partial文件夾下FormCollectionObjectService與IFormCollectionObjectService中編寫
 */
using VOL.System.IRepositories;
using VOL.System.IServices;
using VOL.Core.BaseProvider;
using VOL.Core.Extensions.AutofacManager;
using VOL.Entity.DomainModels;

namespace VOL.System.Services
{
    public partial class FormCollectionObjectService : ServiceBase<FormCollectionObject, IFormCollectionObjectRepository>
    , IFormCollectionObjectService, IDependency
    {
    public FormCollectionObjectService(IFormCollectionObjectRepository repository)
    : base(repository)
    {
    Init(repository);
    }
    public static IFormCollectionObjectService Instance
    {
      get { return AutofacContainerModule.GetService<IFormCollectionObjectService>(); } }
    }
 }
