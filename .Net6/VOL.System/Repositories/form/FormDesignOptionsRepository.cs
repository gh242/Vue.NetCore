/*
 *代碼由框架生成,任何更改都可能導致被代碼生成器覆蓋
 *Repository提供數據庫操作，如果要增加數據庫操作請在當前目錄下Partial文件夾FormDesignOptionsRepository編寫代碼
 */
using VOL.System.IRepositories;
using VOL.Core.BaseProvider;
using VOL.Core.EFDbContext;
using VOL.Core.Extensions.AutofacManager;
using VOL.Entity.DomainModels;

namespace VOL.System.Repositories
{
    public partial class FormDesignOptionsRepository : RepositoryBase<FormDesignOptions> , IFormDesignOptionsRepository
    {
    public FormDesignOptionsRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static IFormDesignOptionsRepository Instance
    {
      get {  return AutofacContainerModule.GetService<IFormDesignOptionsRepository>(); } }
    }
}
