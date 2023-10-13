/*
*所有關於App_News類的業務代碼接口應在此處編寫
*/
using System.Threading.Tasks;
using VOL.Core.Utilities;
using VOL.Entity.DomainModels;

namespace VOL.AppManager.IServices
{
    public partial interface IApp_NewsService
    {
        Task<WebResponseContent> GetDemoPageList();
        /// <summary>
        /// 生成html頁面
        /// </summary>
        /// <param name="news"></param>
        /// <returns></returns>
        Task<WebResponseContent> CreatePage(App_News news);
        /// <summary>
        /// 設置封面
        /// </summary>
        /// <param name="news"></param>
        /// <returns></returns>
        WebResponseContent SetCover(App_News news);
    }
}
