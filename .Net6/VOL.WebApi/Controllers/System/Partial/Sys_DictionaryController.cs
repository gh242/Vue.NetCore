using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using VOL.Core.Controllers.Basic;
using VOL.Core.Extensions;
using VOL.Core.Filters;
using VOL.System.IServices;

namespace VOL.System.Controllers
{
    public partial class Sys_DictionaryController
    {
        [HttpPost, Route("GetVueDictionary"), AllowAnonymous]
        [ApiActionPermission()]
        public IActionResult GetVueDictionary([FromBody] string[] dicNos)
        {
            return Content(Service.GetVueDictionary(dicNos).Serialize());
        }
        /// <summary>
        /// table加載數據後刷新當前table數據的字典項(适用字典數據量比較大的情况)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost, Route("getTableDictionary")]
        public IActionResult GetTableDictionary([FromBody] Dictionary<string, object[]> keyData)
        {
            return Json(Service.GetTableDictionary(keyData));
        }
        /// <summary>
        /// 遠程搜索
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost, Route("getSearchDictionary"), AllowAnonymous]
        public IActionResult GetSearchDictionary(string dicNo, string value)
        {
            return Json(Service.GetSearchDictionary(dicNo, value));
        }

        /// <summary>
        /// 表單設置為遠程查詢，重置或第一次添加表單時，獲取字典的key、value
        /// </summary>
        /// <param name="dicNo"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpPost, Route("getRemoteDefaultKeyValue"), AllowAnonymous]
        public async Task<IActionResult> GetRemoteDefaultKeyValue(string dicNo, string key)
        {
            return Json(await Service.GetRemoteDefaultKeyValue(dicNo, key));
        }
        /// <summary>
        /// 代碼生成器獲取所有字典項(超級管理權限)
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("GetBuilderDictionary")]
        // [ApiActionPermission(ActionRolePermission.SuperAdmin)]
        public async Task<IActionResult> GetBuilderDictionary()
        {
            return Json(await Service.GetBuilderDictionary());
        }

    }
}
