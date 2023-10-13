/*
 *代碼由框架生成,任何更改都可能導致被代碼生成器覆蓋
 *Repository提供數據庫操作，如果要增加數據庫操作請在當前目錄下Partial文件夾test2019Repository編寫代碼
 */
using VOL.AppManager.IRepositories;
using VOL.Core.BaseProvider;
using VOL.Core.EFDbContext;
using VOL.Core.Extensions.AutofacManager;
using VOL.Entity.DomainModels;

namespace VOL.AppManager.Repositories
{
    public partial class test2019Repository : RepositoryBase<test2019> , Itest2019Repository
    {
    public test2019Repository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static Itest2019Repository Instance
    {
      get {  return AutofacContainerModule.GetService<Itest2019Repository>(); } }
    }
}
