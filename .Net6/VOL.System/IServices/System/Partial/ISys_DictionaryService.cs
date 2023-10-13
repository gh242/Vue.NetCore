using System.Collections.Generic;
using System.Threading.Tasks;
using VOL.Core.BaseProvider;
using VOL.Entity.DomainModels;

namespace VOL.System.IServices
{
    public partial interface ISys_DictionaryService
    {
        /// <summary>
        /// 代碼生成器獲取所有字典項(超級管理權限)
        /// </summary>
        /// <returns></returns>
        Task<List<string>> GetBuilderDictionary();
        object GetVueDictionary(string[] dicNos);
        object GetTableDictionary(Dictionary<string, object[]> keyData);
        object GetSearchDictionary(string dicNo, string value);

        /// <summary>
        /// 表單設置為遠程查詢，重置或第一次添加表單時，獲取字典的key、value
        /// </summary>
        /// <param name="dicNo"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<object> GetRemoteDefaultKeyValue(string dicNo, string key);
    }
}

