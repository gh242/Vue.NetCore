/*
 *代碼由框架生成,任何更改都可能導致被代碼生成器覆蓋
 *Repository提供數據庫操作，如果要增加數據庫操作請在當前目錄下Partial文件夾IView_OrderInfoRepository編寫接口
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOL.Core.BaseProvider;
using VOL.Entity.DomainModels;
using VOL.Core.Extensions.AutofacManager;
namespace VOL.Demo.IRepositories
{
    public partial interface IView_OrderInfoRepository : IDependency,IRepository<View_OrderInfo>
    {
    }
}
