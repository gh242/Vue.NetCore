/*
 *接口編寫處...
*如果接口需要做Action的權限驗證，請在Action上使用屬性
*如: [ApiActionPermission("SellOrderList",Enums.ActionPermissionOptions.Search)]
 */
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VOL.Core.Filters;
using VOL.Entity.DomainModels;

namespace VOL.Order.Controllers
{
    public partial class SellOrderListController
    {
        /// <summary>
        /// 導出明細
        /// （重寫權限）將子表的權限指向主表權限
        /// </summary>
        /// <param name="loadData"></param>
        /// <returns></returns>
        [ApiActionPermission("SellOrder", Core.Enums.ActionPermissionOptions.Export)]
        [ApiExplorerSettings(IgnoreApi = false)]
        [HttpPost, Route("Export")]
        public override ActionResult Export([FromBody] PageDataOptions loadData)
        {
            return base.Export(loadData);
        }
        /// <summary>
        /// 導入表數據Excel
        ///  （重寫權限）將子表的權限指向主表權限
        /// </summary>
        /// <param name="fileInput"></param>
        /// <returns></returns>
        [HttpPost, Route("Import")]
        [ApiActionPermission("SellOrder", Core.Enums.ActionPermissionOptions.Import)]
        [ApiExplorerSettings(IgnoreApi = false)]
        public override ActionResult Import(List<IFormFile> fileInput)
        {
            return base.Import(fileInput);
        }
        /// <summary>
        /// 下載導入Excel模板
        /// （重寫權限）將子表的權限指向主表權限
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("DownLoadTemplate")]
        [ApiActionPermission("SellOrder", Core.Enums.ActionPermissionOptions.Import)]
        [ApiExplorerSettings(IgnoreApi = false)]
        public override ActionResult DownLoadTemplate()
        {
            return base.DownLoadTemplate();
        }
    }
}
