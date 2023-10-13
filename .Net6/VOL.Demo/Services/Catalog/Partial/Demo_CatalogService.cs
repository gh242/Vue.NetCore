/*
 *所有關於Demo_Catalog類的業務代碼應在此處編寫
*可使用repository.調用常用方法，獲取EF/Dapper等信息
*如果需要事務請使用repository.DbContextBeginTransaction
*也可使用DBServerProvider.手動獲取數據庫相關信息
*用戶信息、權限、角色等使用UserContext.Current操作
*Demo_CatalogService對增、刪、改查、導入、導出、審核業務代碼擴展參照ServiceFunFilter
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
using VOL.Demo.IRepositories;
using VOL.Core.ManageUser;
using OfficeOpenXml;
using VOL.Demo.IServices;

namespace VOL.Demo.Services
{
    public partial class Demo_CatalogService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDemo_CatalogRepository _repository;//訪問數據庫
        private readonly IDemo_OrderService _orderService;

        [ActivatorUtilitiesConstructor]
        public Demo_CatalogService(
            IDemo_CatalogRepository dbRepository,
            IHttpContextAccessor httpContextAccessor,
            IDemo_OrderService orderService
            )
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _orderService = orderService;
            //多租戶會用到這init代碼，其他情况可以不用
            //base.Init(dbRepository);
        }

        private WebResponseContent webResponse = new WebResponseContent();

        public override PageGridData<Demo_Catalog> GetPageData(PageDataOptions options)
        {
            //_orderService.GetPageData

            //此處是前台提交的原生的查詢條件，這裡可以自己過濾 // 用的比較少
            QueryRelativeList = (List<SearchParameters> parameters) =>
            {

            };

            //if(options.Value.GetInt()==1)
            //{
            //    Console.WriteLine("1");
            //}

            //2020.08.15
            //設置原生查詢的sql語句，這裡必須返回select * 表所有字段
            //（先内部過濾數據，內部調用EF方法FromSqlRaw，自己寫的sql注意sql注入的問題），不會影響介面上提交的查詢
            //string date = DateTime.Now.AddYears(-10).ToString("yyyy-MM-dd");
            string date = DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd");
            //QuerySql = $@"select * from Demo_Catalog  
            //                           where createdate>'{date}'
            //                           and  CatalogCode in (select CatalogCode from Demo_Catalog)
            //                           and CreateID={UserContext.Current.UserId}";

            //QuerySql = $@"select * from Demo_Catalog where createdate > '{date}'";
            //return base.GetPageData(options);

            // repository.DapperContext.ex
            //PageGridData<Demo_Catalog> gridData = new PageGridData<Demo_Catalog>()
            //{
            //    rows = new List<Demo_Catalog>() { },
            //    total = 100,
            //};

            //2020.08.15
            //此處與上面QuerySql只需要實現其中一個就可以了
            //查詢前可以自己設定查詢表達式的條件
            //QueryRelativeExpression = (IQueryable<Demo_Catalog> queryable) =>
            //{
            //    //當前用戶只台操作自己(與下級角色)創建的數據，如：查詢、刪除、修改等操作
            //    //IQueryable<int> userQuery = RoleContext.GetCurrentAllChildUser();
            //    //queryable = queryable.Where(x => x.CreateID == UserContext.Current.UserId || userQuery.Contains(x.CreateID ?? 0));

            //    queryable = queryable.Where(x => x.CreateID == UserContext.Current.UserId);

            //    return queryable;
            //};
            return base.GetPageData(options);
        }



        public override WebResponseContent Add(SaveModel saveDataModel)
        {
            // 在保存數據前的操作，所有數據都驗證通過了，這一步執行完就執行數據庫保存
            AddOnExecuting = (Demo_Catalog catalog, object list) =>
            {
                //if (repository.Exists(x => x.CatalogName == catalog.CatalogName))
                if (repository.Exists(x => x.CatalogCode == catalog.CatalogCode))
                {
                    return webResponse.Error("分類編號已存在");
                }
                return webResponse.OK();
            };


            //此方法中已開啟了事務，如果在此方法中做其他數據庫操作，請不要再開啟事務
            // 在保存數據庫後的操作，此時已進行數據提交，但未提交事務，如果返回false，則會回滾提交
            AddOnExecuted = (Demo_Catalog catalog, object list) =>
            {
                //明細表對象
                // List<SellOrderList> orderLists = list as List<SellOrderList>;

                //if (order.Qty < 10)
                //{  //如果輸入的銷售數量<10，會回滾數據庫
                //    return webResponse.Error("銷售數量必須大於1000");
                //}

                return webResponse.OK("已新建成功,台AddOnExecuted方法返回的消息");
            };

            return base.Add(saveDataModel);
        }

        public override WebResponseContent Update(SaveModel saveModel)
        {

            //編輯方法保存數據庫前處理
            UpdateOnExecuting = (Demo_Catalog catalog, object addList, object updateList, List<object> delKeys) =>
            {
                //repository.DapperContext.ExecuteScalar("select * from ...");
                //repository.DbContext.Set<Sys_User>().Find() // 原生EF
                //repository.DbContext.Set<Sys_User>().Where() // 原生EF
                // 判斷分類編號存不存在
                if (repository.Exists(x => x.CatalogCode == catalog.CatalogCode && x.CatalogId != catalog.CatalogId))
                {
                    return webResponse.Error("分類編號已存在");
                }
                return webResponse.OK();
            };

            //編輯方法保存數據庫後處理
            // 如果要對其他表操作，也可以在這裡寫
            //此方法中已開啟了事務，如果在此方法中做其他數據庫操作，請不要再開啟事務
            // 在保存數據庫後的操作，此時已進行數據提交，但未提交事務，如果返回false，則會回滾提交
            //UpdateOnExecuted = (Demo_Catalog catalog, object addList, object updateList, List<object> delKeys) =>
            //{
            //    //新增的明細
            //    //List<SellOrderList> add = addList as List<SellOrderList>;
            //    //修改的明細
            //    //List<SellOrderList> update = updateList as List<SellOrderList>;
            //    //刪除的行的主鍵
            //    var guids = delKeys?.Select(x => (Guid)x);
            //    return webResponse.OK();
            //};sd

            return base.Update(saveModel);
        }

        /// <summary>
        /// 導出
        /// </summary>
        /// <param name="pageData"></param>
        /// <returns></returns>
        public override WebResponseContent Export(PageDataOptions pageData)
        {
            //設置最大導出的數量
            Limit = 1000;  //默認最大導出5000條
            //指定導出的字段(2020.05.07)
            // 只需導出CatalogCode 及 CatalogName // 默認導出所有欄位
            //ExportColumns = x => new { x.CatalogCode, x.CatalogName };

            //查詢要導出的數據後，在生成excel文件前處理
            //list導出的實體，ignore過濾不導出的字段
            //ExportOnExecuting = (List<Demo_Catalog> list, List<string> ignore) =>
            //{
            //    // 可以修改要導出的資料
            //    list.ForEach(item =>
            //    {
            //        item.CatalogCode = "11";
            //    });
            //    ignore.Add("Remark");  // 忽略導出的欄位
            //    return webResponse.OK();
            //};


            return base.Export(pageData);
        }

        /// <summary>
        /// 下載模板(導入時彈出框中的下載模板)(2020.05.07)
        /// </summary>
        /// <returns></returns>
        public override WebResponseContent DownLoadTemplate()
        {
            //指定導出模板的字段,如果不設置DownLoadTemplateColumns，默認導出查所有頁面上能看到的列(2020.05.07)
            DownLoadTemplateColumns = x => new { x.CatalogCode, x.CatalogName, x.Remark, x.CreateDate };
            return base.DownLoadTemplate();
        }

        /// <summary>
        /// 導入
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public override WebResponseContent Import(List<IFormFile> files)
        {
            //(2020.05.07)
            //設置導入的字段(如果指定了上面導出模板的字段，這裡配置應該與上面DownLoadTemplate方法裡配置一樣)
            //如果不設置導入的字段DownLoadTemplateColumns,默認顯示所有介面上所有可以看到的字段
            DownLoadTemplateColumns = x => new { x.CatalogCode, x.CatalogName, x.Remark, x.CreateDate };

            /// <summary>
            /// 2022.06.20增加原生excel读取方法(導入時可以自定義讀取取excel内容)
            /// string=當前讀取的excel單元格的值
            /// ExcelWorksheet=excel對象
            /// ExcelRange當前excel單元格對象
            /// int=當前讀取的第幾行
            /// int=當前讀取的第幾列
            /// string=返回的值
            /// </summary>
            ImportOnReadCellValue = (string value, ExcelWorksheet worksheet, ExcelRange excelRange, int rowIndex, int columnIndex) =>
            {
                string 表頭列名 = worksheet.Cells[1, columnIndex].Value?.ToString();
                //這裡可以返回處理後的值，值最終寫到model字段上
                return value;
            };


            //導入保存前處理(可以對list設置新的值)
            ImportOnExecuting = (List<Demo_Catalog> list) =>
            {
                //设置webResponse.Code = "-1"會中止後面代碼執行，與返回 webResponse.Error()一樣，區别在於前端提示的是成功或失敗
                //webResponse.Code = "-1";
                //webResponse.Message = "測試強憲返回";
                //return webResponse.OK("ok");

                return webResponse.OK();
            };
            
            //導入後處理(已經寫入到數據庫了)
            
            ImportOnExecuted = (List<Demo_Catalog> list) =>
            {
                return webResponse.OK();
            };
            return base.Import(files);

        }
        
    }
}
