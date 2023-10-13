/*
 *接口編寫處...
*如果接口需要做Action的權限驗證，請在Action上使用屬性
*如: [ApiActionPermission("Sys_DictionaryList",Enums.ActionPermissionOptions.Search)]
 */
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using VOL.Entity.DomainModels;
using VOL.System.IServices;
using VOL.Core.Filters;

namespace VOL.System.Controllers
{
    public partial class Sys_DictionaryListController
    {
        private readonly ISys_DictionaryListService _service;//訪問業務代碼
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Sys_DictionaryListController(
            ISys_DictionaryListService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }
        /// <summary>
        /// 導出明細
        /// （重寫權限）將子表的權限指向主表權限
        /// </summary>
        /// <param name="loadData"></param>
        /// <returns></returns>
        [ApiActionPermission("Sys_Dictionary", Core.Enums.ActionPermissionOptions.Export)]
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
        [ApiActionPermission("Sys_Dictionary", Core.Enums.ActionPermissionOptions.Import)]
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
        [ApiActionPermission("Sys_Dictionary", Core.Enums.ActionPermissionOptions.Import)]
        [ApiExplorerSettings(IgnoreApi = false)]
        public override ActionResult DownLoadTemplate()
        {
            return base.DownLoadTemplate();
        }
    }
}
