/*
*所有關於App_ReportPrice類的業務代碼接口應在此處編寫
*/
using System.Threading.Tasks;
using VOL.Core.BaseProvider;
using VOL.Core.Utilities;
using VOL.Entity.DomainModels;
namespace VOL.AppManager.IServices
{
    public partial interface IApp_ReportPriceService
    {
        /// <summary>
        /// 獲取table1的數據
        /// </summary>
        /// <param name="loadData"></param>
        /// <returns></returns>
        Task<object> GetTable1Data(PageDataOptions loadData);


        /// <summary>
        /// 獲取table2的數據
        /// </summary>
        /// <param name="loadData"></param>
        /// <returns></returns>
        Task<object> GetTable2Data(PageDataOptions loadData);
    }
 }
