/*
 *代碼由框架生成,任何更改都可能導致被代碼生成器覆蓋
 *Repository提供數據庫操作，如果要增加數據庫操作請在當前目錄下Partial文件夾App_ExpertRepository編寫代碼
 */
using VOL.AppManager.IRepositories;
using VOL.Core.BaseProvider;
using VOL.Core.EFDbContext;
using VOL.Core.Extensions.AutofacManager;
using VOL.Entity.DomainModels;

namespace VOL.AppManager.Repositories
{
    public partial class App_ExpertRepository : RepositoryBase<App_Expert> , IApp_ExpertRepository
    {
    public App_ExpertRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static IApp_ExpertRepository Instance
    {
      get {  return AutofacContainerModule.GetService<IApp_ExpertRepository>(); } }
    }
}
