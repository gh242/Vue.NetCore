/*
 *所有關於App_Appointment類的業務代碼應在此處編寫
*可使用repository.調用常用方法，獲取EF/Dapper等信息
*如果需要事務請使用repository.DbContextBeginTransaction
*也可使用DBServerProvider.手動獲取數據庫相關信息
*用戶信息、權限、角色等使用UserContext.Current操作
*App_AppointmentService對增、刪、改查、導入、導出、審核業務代碼擴展參照ServiceFunFilter
*/
using VOL.Core.BaseProvider;
using VOL.Core.Extensions.AutofacManager;
using VOL.Entity.DomainModels;
using System.Linq;
using VOL.Core.Extensions;
namespace VOL.Order.Services
{
    public partial class App_AppointmentService
    {
    }
}
