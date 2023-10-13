/*
 *接口編寫處...
*如果接口需要做Action的權限驗證，請在Action上使用屬性
*如: [ApiActionPermission("App_Expert",Enums.ActionPermissionOptions.Search)]
 */
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VOL.AppManager.Repositories;
using VOL.Core.Extensions;
using VOL.Entity.DomainModels;

namespace VOL.AppManager.Controllers
{
    public partial class App_ExpertController
    {
        //後台App_ExpertController中添加代碼，返回table數據
        [HttpPost, Route("getSelectorDemo")]
        public IActionResult GetSelectorDemo([FromBody] PageDataOptions options)
        {
            //1.可以直接調用框架的GetPageData查詢
            // PageGridData<App_Expert> data = App_ExpertService.Instance.GetPageData(options);
            //return Json(data);

            //2.下面這裡演示手動解析查詢返回
            //解析查詢條件，查詢條件都放在Wheres中，也可以在前端自定義wheres格式
            List<SearchParameters> wheres = options?.Wheres?.DeserializeObject<List<SearchParameters>>();
            //生成查詢條件(這裡可以不用EF，自己寫原生sql)
            IQueryable<App_Expert> query = App_ExpertRepository.Instance.FindAsIQueryable(x => true);
            if (wheres != null)
            {
                string searchValue = wheres.Where(x => x.Name == "expertName").Select(s => s.Value).FirstOrDefault();
                //WhereNotEmpty方法要更新EntityProperties.cs否則用下面的if (!string.IsNullOrEmpty(searchKey))
                //query = query.WhereNotEmpty(x => x.ExpertName, searchValue);
                if (!string.IsNullOrEmpty(searchValue))
                {
                    query = query.Where(x => x.ExpertName.Contains(searchValue));
                }
            }
            var data = new
            {
                total = query.Count(), //查詢到的總行數
                rows = query.OrderByDescending(x => x.CreateDate)//返回table列表
                .Select(s => new
                {
                    s.ExpertId,
                    s.ExpertName,
                    s.HeadImageUrl,
                    s.Resume,
                    s.Enable
                })
                .Skip(options.Page * (options.Page - 1))
                .Take(options.Rows).ToList()
            };
            return Json(data);
            //上面Json(data);返回的字段首字母是小寫的,也可以使用JsonNormal返回原大小寫不變
            // return JsonNormal(data);
        }
    }
}
