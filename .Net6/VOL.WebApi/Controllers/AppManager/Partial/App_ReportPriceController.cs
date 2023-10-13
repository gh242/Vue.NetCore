/*
 *接口編寫處...
*如果接口需要做Action的權限驗證，請在Action上使用屬性
*如: [ApiActionPermission("App_ReportPrice",Enums.ActionPermissionOptions.Search)]
 */
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VOL.Core.Enums;
using VOL.Core.Filters;
using VOL.Entity.DomainModels;

namespace VOL.AppManager.Controllers
{
    public partial class App_ReportPriceController
    {
        //ApiActionPermission("App_ReportPrice", ActionPermissionOptions.Search)設置查詢表的權限，如果不填寫只要登入了都可以查詢
        /// <summary>
        /// 獲取table1的數據
        /// </summary>
        /// <param name="loadData"></param>
        /// <returns></returns>
        [Route("getTable1Data"),HttpPost, ApiActionPermission("App_ReportPrice", ActionPermissionOptions.Search)]
        public async Task<IActionResult> GetTable1Data([FromBody] PageDataOptions loadData)
        {
            return Json(await Service.GetTable1Data(loadData));
        }

        /// <summary>
        /// 獲取table1的數據
        /// </summary>
        /// <param name="loadData"></param>
        /// <returns></returns>
        [Route("getTable2Data"), HttpPost, ApiActionPermission("App_ReportPrice", ActionPermissionOptions.Search)]
        public async Task<IActionResult> GetTable2Data([FromBody] PageDataOptions loadData)
        {
            return Json(await Service.GetTable2Data(loadData));
        }
    }
}
