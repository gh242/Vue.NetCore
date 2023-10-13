/*
 *代碼由框架生成,任何更改都可能導致被代碼生成器覆蓋
 *Repository提供數據庫操作，如果要增加數據庫操作請在當前目錄下Partial文件夾Demo_CustomerRepository編寫代碼
 */
using VOL.Demo.IRepositories;
using VOL.Core.BaseProvider;
using VOL.Core.EFDbContext;
using VOL.Core.Extensions.AutofacManager;
using VOL.Entity.DomainModels;

namespace VOL.Demo.Repositories
{
    public partial class Demo_CustomerRepository : RepositoryBase<Demo_Customer> , IDemo_CustomerRepository
    {
    public Demo_CustomerRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static IDemo_CustomerRepository Instance
    {
      get {  return AutofacContainerModule.GetService<IDemo_CustomerRepository>(); } }
    }
}
