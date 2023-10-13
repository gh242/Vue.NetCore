/*
 *所有關於FormCollectionObject類的業務代碼應在此處編寫
*可使用repository.調用常用方法，獲取EF/Dapper等信息
*如果需要事務請使用repository.DbContextBeginTransaction
*也可使用DBServerProvider.手動獲取數據庫相關信息
*用戶信息、權限、角色等使用UserContext.Current操作
*FormCollectionObjectService對增、刪、改查、導入、導出、審核業務代碼擴展參照ServiceFunFilter
*/
using VOL.Core.BaseProvider;
using VOL.Core.Extensions.AutofacManager;
using VOL.Entity.DomainModels;
using System.Linq;
using VOL.Core.Utilities;
using System.Linq.Expressions;
using VOL.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using VOL.System.IRepositories;
using System.Collections.Generic;
using VOL.Core.Configuration;
using VOL.Core.Services;
using System;
using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.Style;
using System.Drawing;

namespace VOL.System.Services
{
    public partial class FormCollectionObjectService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFormCollectionObjectRepository _repository;//訪問數據庫
        private readonly IFormDesignOptionsRepository _designOptionsRepository;
        [ActivatorUtilitiesConstructor]
        public FormCollectionObjectService(
            IFormCollectionObjectRepository dbRepository,
            IHttpContextAccessor httpContextAccessor,
             IFormDesignOptionsRepository designOptionsRepository
            )
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _designOptionsRepository = designOptionsRepository;
        }

        public override WebResponseContent Export(PageDataOptions pageData)
        {
            string path = null;
            string fileName = null;
            WebResponseContent webResponse = new WebResponseContent();
            ExportOnExecuting = (List<FormCollectionObject> list, List<string> columns) =>
            {
                var formId = list[0].FormId;
                var data = _designOptionsRepository.FindAsIQueryable(x => x.FormId == formId)
                   .Select(s => new { s.Title, s.FormConfig }).FirstOrDefault();
                try
                {
                    List<FormOptions> formObj = data.FormConfig.DeserializeObject<List<FormOptions>>();
                    List<Dictionary<string, object>> listDic = new List<Dictionary<string, object>>();
                    foreach (var item in list)
                    {
                            Dictionary<string, object> dic = new Dictionary<string, object>();
                            var formData = item.FormData.DeserializeObject<Dictionary<string, string>>();
                            dic.Add("標題", data.Title);
  
                            dic.Add("提交人", item.Creator);
                            dic.Add("提交時間", item.CreateDate.ToString("yyyy-MM-dd HH:mm:sss"));
                            foreach (var obj in formObj)
                            {
                                dic.Add(obj.Title, formData.Where(x => x.Key == obj.Field).Select(s => s.Value).FirstOrDefault());
                            }
                            listDic.Add(dic);
                    }
                    fileName = data.Title + ".xlsx";
                    path = EPPlusHelper.ExportGeneralExcel(listDic, fileName);
                }
                catch (Exception ex)
                {
                    Logger.Error($"解析表單出錯：{data.Title},表單配置：{data.FormConfig},{ex.Message}");
                    return webResponse.Error("獲取表單出錯");
                }
                webResponse.Code = "-1";
                return webResponse.OK(null, path.EncryptDES(AppSetting.Secret.ExportFile));
            };
            return base.Export(pageData);
        } 
    }

    public class FormOptions
    {
        public string Field { get; set; }

        public string Title { get; set; }

        public string Type { get; set; }
    }
}
