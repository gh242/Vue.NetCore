/*
 *所有關於SellOrder類的業務代碼應在此處編寫
*/
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using VOL.Core.BaseProvider;
using VOL.Core.Enums;
using VOL.Core.Extensions;
using VOL.Core.ManageUser;
using VOL.Core.UserManager;
using VOL.Core.Utilities;
using VOL.Core.WorkFlow;
using VOL.Entity.DomainModels;
using VOL.Order.IRepositories;
using VOL.Order.Repositories;

namespace VOL.Order.Services
{
    public partial class SellOrderService
    {
        public string GetServiceDate()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss");
        }
        //此SellOrderService.cs類由代碼生成器生成，默認是沒有任何代碼，如果需要寫業務代碼，請在此類中實現
        //如果默認的增、刪、改、查、導入、導出、審核滿足不了業務，請參考下面的方法進行業務代碼擴展(擴展代碼是對ServiceFunFilter.cs的實現)
        WebResponseContent webResponse = new WebResponseContent();
        private IHttpContextAccessor _httpContextAccessor;
        private ISellOrderRepository _repository;
        [ActivatorUtilitiesConstructor]
        public SellOrderService(IHttpContextAccessor httpContextAccessor, ISellOrderRepository repository)
            : base(repository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = repository;
            //2020.08.15
            //開啟數據隔離功能,開啟後會對查詢、導出、刪除、編輯功能同時生效
            //如果只需要對某個功能生效，如編輯，則在重寫編輯方法中設置 IsMultiTenancy = true;
            // IsMultiTenancy = true;
        }

        //將前端table組件提交的查詢參數轉換為表達式
        public IQueryable<SellOrder> Test(PageDataOptions options)
        {

            ////1、對象轉換為字符串
            //Sys_User user = new Sys_User();
            //string userText=  user.Serialize();

            ////2、字符串轉換為對象(上面的json字符串userText轉換為對象)
            //user = userText.DeserializeObject<Sys_User>();


            ////list對象序列化與反序列化
            //List<Sys_User> list = new List<Sys_User>();
            //list.Add(new Sys_User() { UserName = "test" });

            //userText = list.Serialize();

            ////字符串轉換為對象(上面的json字符串userText轉換為對象)
            //list = userText.DeserializeObject<List<Sys_User>>();


            //options.Wheres;為前端提交的查詢條件
            //手動獲取查詢條件
            //List<SearchParameters> parameters= options.Wheres.DeserializeObject<List<SearchParameters>>();

            IQueryable<SellOrder> query = base.GetPageDataQueryFilter(options);
            //或者調用其他表的轉換
            //SellOrderService.Instance.GetPageDataQueryFilter(options);
            return query;
        }

        /// <summary>
        /// 2023.02.03增加or查詢條件示例
        /// 注意：如果有導出功能，GetPageData方法内的代碼在下面的export方法里需要同樣的複製一份
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        //public override PageGridData<SellOrder> GetPageData(PageDataOptions options)
        //{
        //    System.Linq.Expressions.Expression<Func<SellOrder, bool>> orFilter = null;
        //    QueryRelativeList = (List<SearchParameters> parameters) =>
        //    {
        //        //方式1：動態生成or查詢條件
        //        foreach (var item in parameters)
        //        {
        //            if (!string.IsNullOrEmpty(item.Value))
        //            {
        //                if (orFilter == null&&(item.Name == "TranNo"|| item.Name == "SellNo")){ orFilter = x => false; }

        //                //注意:這裡只需要判斷or查詢的字段，其他的字段不需要處理
        //                //這裡必須拷貝value值
        //                string value = item.Value;
        //                if (item.Name == "TranNo")
        //                {
        //                    //進行or模糊查詢
        //                    orFilter = orFilter.Or(x => x.TranNo.Contains(value));
        //                    //清空原來的數據
        //                    item.Value = null;
        //                }
        //                else if (item.Name == "SellNo")
        //                {
        //                    //進行or等於查詢
        //                    orFilter = orFilter.Or(x => x.SellNo == value);
        //                    //清空原來的數據
        //                    item.Value = null;
        //                }
        //            }
        //        }
        //        ///方式2：原生sql查詢,需要自己處理sql注入問題(不建議使用此方法)
        //        //string sql = null;
        //        //foreach (var item in parameters)
        //        //{
        //        //    if (!string.IsNullOrEmpty(item.Value))
        //        //    {
        //        //        if (sql == null)
        //        //        {
        //        //            sql = "where 1=2";
        //        //        }
        //        //        string value = item.Value;
        //        //        //清空原來的數據
        //        //        item.Value = null;
        //        //        if (item.Name == "TranNo")
        //        //        {
        //        //            sql += $" or TranNo='{value}'";
        //        //            //清空原來的數據
        //        //            item.Value = null;
        //        //        }
        //        //        else if (item.Name == "SellNo")
        //        //        {
        //        //            sql += $" or SellNo='{value}'";
        //        //        }
        //        //    }
        //        //}
        //        //QuerySql = "select * from sellorder " + sql;
        //    };

        //    QueryRelativeExpression = (IQueryable<SellOrder> queryable) =>
        //    {
        //        if (orFilter != null)
        //        {
        //            queryable = queryable.Where(orFilter);
        //        }
        //        return queryable;
        //    };
        //    return base.GetPageData(options);
        //}


        //查詢
        public override PageGridData<SellOrder> GetPageData(PageDataOptions options)
        {
            //options.Value可以從前台查詢的方法提交一些其他參數放到value裡面
            //前端提交方式，見文欄：組件api->viewgrid組件裡面的searchBefore方法
            object extraValue = options.Value;

            //此處是從前台提交的原生的查詢條件，這裡可以自己過濾
            QueryRelativeList = (List<SearchParameters> parameters) =>
            {

            };

            //2020.08.15
            //設置原生查詢的sql語句，這裡必須返回select * 表所有字段
            //（先内部過濾數據,内部調用EF方法FromSqlRaw,自己寫的sql注意sql注入的問題），不會影響介面上提交的查詢
            /*  
             *  string date = DateTime.Now.AddYears(-10).ToString("yyyy-MM-dd");
                QuerySql = $@"select * from SellOrder  
                                       where createdate>'{date}'
                                           and  Order_Id in (select Order_Id from SellOrderList)
                                           and CreateID={UserContext.Current.UserId}";
            */

            //2020.08.15
            //此處與上面QuerySql只需要實現其中一個就可以了
            //查詢前可以自已設定查詢表達式的條件
            QueryRelativeExpression = (IQueryable<SellOrder> queryable) =>
            {
                //當前用戶只能操作自己(與下級角色)創建的數據,如:查詢、刪除、修改等操作
                //IQueryable<int> userQuery = RoleContext.GetCurrentAllChildUser();
                //queryable = queryable.Where(x => x.CreateID == UserContext.Current.UserId || userQuery.Contains(x.CreateID ?? 0));
                return queryable;
            };

            //指定多個字段進行排序
            OrderByExpression = x => new Dictionary<object, QueryOrderBy>() {
                { x.CreateDate,QueryOrderBy.Desc },
                { x.SellNo,QueryOrderBy.Asc}
            };

            //int a = 1;
            ////指定多個字段按條件進行排序（需要2021.07.04更新LambdaExtensions類後才能使用）
            //OrderByExpression = x => new Dictionary<object, QueryOrderBy>() {
            //    { x.CreateDate,QueryOrderBy.Desc },
            //    { x.SellNo,a==1?QueryOrderBy.Desc:QueryOrderBy.Asc}
            //};


            //查詢完成後，在返回頁面前可對查詢的數據進行操作
            GetPageDataOnExecuted = (PageGridData<SellOrder> grid) =>
            {
                //可對查詢的結果的數據操作
                List<SellOrder> sellOrders = grid.rows;
            };
            //查詢table介面顯示求和
            SummaryExpress = (IQueryable<SellOrder> queryable) =>
            {
                return queryable.GroupBy(x => 1).Select(x => new
                {
                    //AvgPrice注意大小寫和數據庫字段大小寫一樣
                    Qty = x.Sum(o => o.Qty).ToString("f2")
                })
                .FirstOrDefault();
            };

            return base.GetPageData(options);
        }
        /// <summary>
        /// 設置彈出框明細表的合計信息
        /// </summary>
        /// <typeparam name="detail"></typeparam>
        /// <param name="queryeable"></param>
        /// <returns></returns>
        protected override object GetDetailSummary<detail>(IQueryable<detail> queryeable)
        {
            return (queryeable as IQueryable<SellOrderList>).GroupBy(x => 1).Select(x => new
            {
                //Weight/Qty注意大小寫和數據庫字段大小寫一樣
                Weight = x.Sum(o => o.Weight),
                Qty = x.Sum(o => o.Qty)
            }).FirstOrDefault();
        }

        /// <summary>
        /// 查詢業務代碼編寫(從表(明細表查詢))
        /// </summary>
        /// <param name="pageData"></param>
        /// <returns></returns>
        public override object GetDetailPage(PageDataOptions pageData)
        {
            //自定義查詢胆細表

            ////明細表自定義查詢方式一：EF
            //var query = SellOrderListRepository.Instance.IQueryablePage<SellOrderList>(
            //     pageData.Page,
            //     pageData.Rows,
            //     out int count,
            //     x => x.Order_Id == pageData.Value.GetGuid(),
            //      orderBy: x => new Dictionary<object, QueryOrderBy>() { { x.CreateDate, QueryOrderBy.Desc } }
            //    );
            //PageGridData<SellOrderList> detailGrid = new PageGridData<SellOrderList>();
            //detailGrid.rows = query.ToList();
            //detailGrid.total = count;

            ////明細表自定義查詢方式二：dapper
            //string sql = "select count(1) from SellOrderList where Order_Id=@orderId";
            //detailGrid.total = repository.DapperContext.ExecuteScalar(sql, new { orderId = pageData.Value }).GetInt();

            //sql = @$"select * from (
            //              select *,ROW_NUMBER()over(order by createdate desc) as rowId 
            //           from SellOrderList where Order_Id=@orderId
            //        ) as s where s.rowId between {((pageData.Page - 1) * pageData.Rows + 1)} and {pageData.Page * pageData.Rows} ";
            //detailGrid.rows = repository.DapperContext.QueryList<SellOrderList>(sql, new { orderId = pageData.Value });

            //return detailGrid;

            return base.GetDetailPage(pageData);
        }

        /// <summary>
        /// 新建
        /// </summary>
        /// <param name="saveDataModel"></param>
        /// <returns></returns>
        public override WebResponseContent Add(SaveModel saveDataModel)
        {
            //此處saveModel是從前台提交的原生數據，可對數據進修改過濾
            AddOnExecute = (SaveModel saveModel) =>
            {
                //如果返回false,後面代碼不會再執行
                return webResponse.OK();
            };
            // 在保存數據庫前的操作，所有數據都驗證通過了，這一步執行完就執行數據庫保存
            AddOnExecuting = (SellOrder order, object list) =>
            {
                //明細表對象
                List<SellOrderList> orderLists = list as List<SellOrderList>;

                //自定義邏輯
                if (orderLists == null || orderLists.Count == 0)
                {//如果沒有介面上沒有填寫明細，則中斷執行
                    return webResponse.Error("必須填寫明細數據");
                }
                if (orderLists.Exists(x => x.Qty <= 20))
                    return webResponse.Error("明細數量必須大於20");

                //設置webResponse.Code = "-1"會中止後面代碼執行，與返回 webResponse.Error()一樣，區別在於前端提示的是成功或失敗
                //webResponse.Code = "-1";
                // webResponse.Message = "測試强制返回";
                //return webResponse.OK("ok");

                return webResponse.OK();
            };

            //此方法中已開啟了事務，如果在此方法中做其他數據庫操作，請不要再開啟事務
            // 在保存數據庫後的操作，此時已進行數據提交，但未提交事務，如果返回false，則會回滾提交
            AddOnExecuted = (SellOrder order, object list) =>
            {
                //明細表對象
                // List<SellOrderList> orderLists = list as List<SellOrderList>;

                if (order.Qty < 10)
                {  //如果輸入的銷售數量<10，會回滾數據庫
                    return webResponse.Error("銷售數量必須大於1000");
                }

                return webResponse.OK("已新建成功,台AddOnExecuted方法返回的消息");
            };


            //新建的數據進入審批流程前處理，
            AddWorkFlowExecuting = (SellOrder order) =>
            {
                //返回false，當前數據不會進入審批流程
                return true;
            };

            //新建的數據寫入審批流程後,第二個參數為審批人的用戶id
            AddWorkFlowExecuted = (SellOrder order, List<int> userIds) =>
            {
                //這裡可以做發郵件通知
                //var userInfo = repository.DbContext.Set<Sys_User>()
                //                .Where(x => userIds.Contains(x.User_Id))
                //                .Select(s => new { s.User_Id, s.UserTrueName, s.Email, s.PhoneNo }).ToList();

                //發送郵件方法
                //MailHelper.Send()
            };

            return base.Add(saveDataModel);
        }
        /// <summary>
        /// 編輯操作
        /// </summary>
        /// <param name="saveModel"></param>
        /// <returns></returns>
        public override WebResponseContent Update(SaveModel saveModel)
        {
            //注意：如果要給其他字段設置值，請在此處設置,如：（代碼生成器上將字段編輯行設置為0，然後點生成model）
            //saveModel.MainData["字段"] = "值";

            //此處saveModel是從前台提交的原生數據，可對數據進修改過濾
            UpdateOnExecute = (SaveModel model) =>
            {
                ////這裡的設置配合下面order.Remark = "888"代碼位置使用
                // saveModel.MainData.TryAdd("Remark", "1231");

                //如果不想前端提交某些可以編輯的字段的值,直接移除字段
                // saveModel.MainData.Remove("字段");

                //如果返回false,後面代碼不會再執行
                return webResponse.OK();

            };

            //編輯方法保存數據庫前處理
            UpdateOnExecuting = (SellOrder order, object addList, object updateList, List<object> delKeys) =>
              {
                  if (order.TranNo == "2019000001810001")
                  {
                      //如果設置code=-1會强制返回，不再繼續後面的操作,2021.07.04更新LambdaExtensions文件後才可以使用此屬性
                      //webResponse.Code = "-1";
                      // webResponse.Message = "測試强制返回";
                      //return webResponse.OK();
                      return webResponse.Error("不能更新此[" + order.TranNo + "]單號");
                  }

                  ////如果要手動設置某些字段的值,值不是前端提交的（代碼生成器裡面編輯行必須設置為0並生成model）,如Remark字段:
                  ////注意必須設置上面saveModel.MainData.TryAdd("Remark", "1231")
                  //order.Remark = "888";


                  //新增的明細表
                  List<SellOrderList> add = addList as List<SellOrderList>;
                  //修改的明細表
                  List<SellOrderList> update = updateList as List<SellOrderList>;
                  //刪除明細表Id
                  var guids = delKeys?.Select(x => (Guid)x);

                  //設置webResponse.Code = "-1"會中止後面代碼執行，與返回 webResponse.Error()一樣，區別在於前端提示的是成功或失敗
                  //webResponse.Code = "-1";
                  // webResponse.Message = "測試强制返回";
                  //return webResponse.OK("ok");

                  return webResponse.OK();
              };

            //編輯方法保存數據庫後處理
            //此方法中已開啟了事務，如果在此方法中做其他數據庫操作，請不要再開啟事務
            // 在保存數據庫後的操作，此時已進行數據提交，但未提交事務，如果返回false，則會回滾提交
            UpdateOnExecuted = (SellOrder order, object addList, object updateList, List<object> delKeys) =>
              {
                  //新增的明細
                  List<SellOrderList> add = addList as List<SellOrderList>;
                  //修改的明細
                  List<SellOrderList> update = updateList as List<SellOrderList>;
                  //刪除的行的主鍵
                  var guids = delKeys?.Select(x => (Guid)x);
                  return webResponse.OK();
              };

            return base.Update(saveModel);
        }

        /// <summary>
        /// 刪除
        /// </summary>
        /// <param name="keys">刪除的行的主鍵</param>
        /// <param name="delList">刪除時是否將明細也刪除</param>
        /// <returns></returns>
        public override WebResponseContent Del(object[] keys, bool delList = true)
        {
            //刪除前處理
            //刪除的行的主鍵
            DelOnExecuting = (object[] _keys) =>
            {
                return webResponse.OK();
            };
            //刪除後處理
            //刪除的行的主鍵
            DelOnExecuted = (object[] _keys) =>
             {
                 return webResponse.OK();
             };
            return base.Del(keys, delList);
        }
        public override WebResponseContent Audit(object[] keys, int? auditStatus, string auditReason)
        {
            //status當前審批狀態,lastAudit是否最後一個審批節點
            AuditWorkFlowExecuting = (SellOrder order, AuditStatus status, bool lastAudit) =>
            {
                return webResponse.OK();
            };
            //status當前審批狀態,nextUserIds下一個節點審批人的帳號id(可以從sys_user表中查詢用戶具體信息),lastAudit是否最後一個審批節點
            AuditWorkFlowExecuted = (SellOrder order, AuditStatus status, List<int> nextUserIds, bool lastAudit) =>
            {
                //lastAudit=true時，流程已經結束 
                if (!lastAudit)
                {
                    //這裡可以給下一批審批發送郵件通知
                    //var userInfo = repository.DbContext.Set<Sys_User>()
                    //             .Where(x => nextUserIds.Contains(x.User_Id))
                    //             .Select(s => new { s.User_Id, s.UserTrueName, s.Email, s.PhoneNo }).ToList();
                }


                //審批流程回退功能，回到第一個審批人重新審批(重新生成審批流程)
                //if (status==AuditStatus.審核未通過||status==AuditStatus.駁回)
                //{
                //    base.RewriteFlow(order);
                //}

                return webResponse.OK();
            };


            //審核保存前處理(不是審批流程)
            AuditOnExecuting = (List<SellOrder> order) =>
            {
                return webResponse.OK();
            };
            //審核後處理(不是審批流程)
            AuditOnExecuted = (List<SellOrder> order) =>
            {
                return webResponse.OK();
            };



            return base.Audit(keys, auditStatus, auditReason);
        }

        /// <summary>
        /// 導出
        /// </summary>
        /// <param name="pageData"></param>
        /// <returns></returns>
        public override WebResponseContent Export(PageDataOptions pageData)
        {
            //設置最大導出的數量
            Limit = 1000;
            //指定導出的字段(2020.05.07)
            ExportColumns = x => new { x.SellNo, x.TranNo, x.CreateDate };

            //查詢要導出的數據後，在生成excel文件前處理
            //list導出的實體，ignore過濾不導出的字段
            ExportOnExecuting = (List<SellOrder> list, List<string> ignore) =>
            {
                return webResponse.OK();
            };

            return base.Export(pageData);
        }

        /// <summary>
        /// 下載模板(導入時彈出框中的下載模板)(2020.05.07)
        /// </summary>
        /// <returns></returns>
        public override WebResponseContent DownLoadTemplate()
        {
            //指定導出模板的字段,如果不設置DownLoadTemplateColumns，默認導出查所有頁面上能看到的列(2020.05.07)
            DownLoadTemplateColumns = x => new { x.SellNo, x.TranNo, x.Remark, x.CreateDate };
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
            //設置導入的字段(如果指定了上面導出模板的字段，這裡配置應該與上面DownLoadTemplate方法里配置一樣)
            //如果不設置導入的字段DownLoadTemplateColumns,默認顯示所有介面上所有可以看到的字段
            DownLoadTemplateColumns = x => new { x.SellNo, x.TranNo, x.Remark, x.CreateDate };


            /// <summary>
            /// 2022.06.20增加原生excel讀取方法(導入時可以自定義讀取excel内容)
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
                //這裡可以返回處理後的值，值最終寫入到model字段上
                return value;
            };

            //導入保存前處理(可以對list設置新的值)
            ImportOnExecuting = (List<SellOrder> list) =>
            {
                //設置webResponse.Code = "-1"會中止後面代碼執行，與返回 webResponse.Error()一樣，區別在於前端提示的是成功或失敗
                //webResponse.Code = "-1";
                //webResponse.Message = "測試强制返回";
                //return webResponse.OK("ok");

                return webResponse.OK();
            };

            //導入後處理(已經寫入到數據庫了)
            ImportOnExecuted = (List<SellOrder> list) =>
            {
                return webResponse.OK();
            };
            return base.Import(files);
        }

        public override WebResponseContent Upload(List<IFormFile> files)
        {
            //自定義上傳文件路徑(目前只支持配置相對路徑，默認上傳到wwwwroot下)
            //2022.10.07更新ServiceBase.cs、ServiceFunFilter.cs後才能使用
            UploadFolder = $"test/{DateTime.Now.ToString("yyyyMMdd")}";
            return base.Upload(files);
        }

    }
}
