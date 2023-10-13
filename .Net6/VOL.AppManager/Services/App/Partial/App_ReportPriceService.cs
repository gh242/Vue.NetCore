/*
 *所有關於App_ReportPrice類的業務代碼應在此處編寫
*可使用repository.調用常用方法，獲取EF/Dapper等信息
*如果需要事務請使用repository.DbContextBeginTransaction
*也可使用DBServerProvider.手動獲取數據庫相關信息
*用戶信息、權限、角色等使用UserContext.Current操作
*App_ReportPriceService對增、刪、改查、導入、導出、審核業務代碼擴展參照ServiceFunFilter
*/
using VOL.Core.BaseProvider;
using VOL.Core.Extensions.AutofacManager;
using VOL.Entity.DomainModels;
using System.Linq;
using VOL.Core.Extensions;
using VOL.Core.Utilities;
using System.Threading.Tasks;
using VOL.AppManager.Repositories;
using System.Collections.Generic;
using VOL.Core.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using VOL.AppManager.IRepositories;

namespace VOL.AppManager.Services
{
    public partial class App_ReportPriceService
    {

        //參照App_ReportPrice.js與App_ReportPriceModelBody.vue文件
        //App_ReportPrice.js為代碼生成的擴展文件
        //App_ReportPriceModelBody.vue為自己創建的擴展文件，裡面可以寫任意代碼



        /// <summary>
        /// 獲取table1的數據
        /// </summary>
        /// <param name="loadData"></param>
        /// <returns></returns>
        public async Task<object> GetTable1Data(PageDataOptions loadData)
        {
            //App_ReportPriceModelBody.vue中loadTableBefore方法查詢前給loadData.Value寫入的值
            object value = loadData.Value;
            //獲取查詢到的總和數
            int total = await App_NewsRepository.Instance.FindAsIQueryable(x => 1 == 1).CountAsync();

            var data = await App_NewsRepository.Instance
                //這裡可以自己查詢條件，從 loadData.Value找前台自定義傳的查詢條件
                .FindAsIQueryable(x => 1 == 1)
                //分頁
                .TakeOrderByPage(1, 10, x => new Dictionary<object, QueryOrderBy>() { { x.CreateDate, QueryOrderBy.Desc } })
                .Select(s => new { s.Id, s.Title, s.ImageUrl, s.Enable, s.CreateDate, s.DailyRecommend })
                .ToListAsync();
            object gridData = new { rows = data, total };
            return gridData;
        }

        /// <summary>
        /// 獲取table2的數據
        /// </summary>
        /// <param name="loadData"></param>
        /// <returns></returns>
        public async Task<object> GetTable2Data(PageDataOptions loadData)
        {
            //App_ReportPriceModelBody.vue中loadTableBefore方法查詢前給loadData.Value寫入的值
            //獲取查詢到的總和數
            int total = await repository.DbContext.Set<App_Appointment>().Where(x => 1 == 1).CountAsync();
            //從 loadData.Value取查詢條件，分頁等信息
            //這裡可以自己查詢條件，從 loadData.Value找前台自定義傳的查詢條件
            var data = await repository.DbContext.Set<App_Appointment>().Where(x => 1 == 1)
                //分頁
                .TakeOrderByPage(1, 10, x => new Dictionary<object, QueryOrderBy>() { { x.CreateDate, QueryOrderBy.Desc } })
                .Select(s => new { s.Id, s.Name, s.PhoneNo, s.Describe, s.CreateDate })
                .ToListAsync();
            object gridData = new { rows = data, total };
            return gridData;
        }

        WebResponseContent _webResponse = new WebResponseContent();

        /// <summary>
        /// 驗證提交的從表參數
        /// saveModel.Extra在App_ReportPrice.js中addBefore/updateBefore新建與編輯時提交到的後台
        /// </summary>
        /// <param name="saveModel"></param>
        /// <returns></returns>
        private WebResponseContent ValidateExtra(SaveModel saveModel)
        {
            if (saveModel == null || saveModel.Extra == null) return _webResponse.Error("請提交參數");
            try
            {
                TableExtra tableExtra = saveModel.Extra.ToString().DeserializeObject<TableExtra>();
                if (tableExtra == null
                    || tableExtra.Table1List == null
                    || tableExtra.Table1List.Count == 0
                    || tableExtra.Table1List == null
                    || tableExtra.Table2List.Count == 0)
                {
                    return _webResponse.Error("請提交從表1與從表2的參數");
                }
                //校驗 從表1字段:Title,CreateDate必填 
                _webResponse = tableExtra.Table1List.ValidationEntityList(x => new { x.Title, x.CreateDate });
                if (!_webResponse.Status)
                {
                    _webResponse.Message = "從表1：" + _webResponse.Message;
                    return _webResponse;
                }
                //校驗 從表2字段:Describe, Name,PhoneNo 必填 
                _webResponse = tableExtra.Table2List.ValidationEntityList(x => new { x.Describe, x.Name, x.PhoneNo });
                if (!_webResponse.Status)
                {
                    _webResponse.Message = "從表2：" + _webResponse.Message;
                    return _webResponse;
                }
                //校驗成功返回
                return _webResponse.OK(null, tableExtra);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return _webResponse.Error("參數不正確");
            }
        }


        /// <summary>
        /// 自定義保存從表數據邏輯
        /// </summary>
        /// <param name="saveDataModel"></param>
        /// <returns></returns>
        public override WebResponseContent Add(SaveModel saveDataModel)
        {
            //校驗從表配置
            ValidateExtra(saveDataModel);
            if (!_webResponse.Status)
            {
                return _webResponse;
            }
            //取出校驗完成後的從表1.2的數據
            TableExtra tableExtra = _webResponse.Data as TableExtra;


            //保存到數據庫前
            AddOnExecuting = (App_ReportPrice price, object obj) =>
            {
                return WebResponseContent.Instance.OK();
            };
            //App_ReportPrice 此處已經提交了數據庫，處於事務中
            AddOnExecuted = (App_ReportPrice price, object obj) =>
            {
                //在此操作tableExtra從表信息
                return WebResponseContent.Instance.OK();
            };
            return base.Add(saveDataModel);
        }

        /// <summary>
        /// 自定義更新從表操作
        /// </summary>
        /// <param name="saveModel"></param>
        /// <returns></returns>
        public override WebResponseContent Update(SaveModel saveModel)
        {
            //校驗從表配置
            ValidateExtra(saveModel);
            if (!_webResponse.Status)
            {
                return _webResponse;
            }
            //取出校驗完成後的從表1.2的數據
            TableExtra tableExtra = _webResponse.Data as TableExtra;

            //保存到數據庫前
            UpdateOnExecuting = (App_ReportPrice price, object obj, object obj2, List<object> list) =>
           {
               return WebResponseContent.Instance.OK();
           };

            //App_ReportPrice 此處已經提交了數據庫，處於事務中
            UpdateOnExecuted = (App_ReportPrice price, object obj, object obj2, List<object> list) =>
            {
                //在此操作tableExtra從表信息
                List<App_News> newsList = tableExtra.Table1List.Select(s => new App_News
                {
                    Id = s.Id ?? 0,
                    Title = s.Title,
                    ImageUrl = s.ImageUrl
                }).ToList();

                //id=0的默認為新增的數據
                List<App_News> addList = newsList.Where(x => x.Id == 0).ToList();
                //設置默認創建人信息
                addList.ForEach(x => { x.SetCreateDefaultVal(); });

                //獲取所有編輯行
                List<int> editIds = newsList.Where(x => x.Id > 0).Select(s => s.Id).ToList();
                addList.ForEach(x => { x.SetModifyDefaultVal(); });
                //從數據庫查詢編輯的行是否存在，如果數據庫不存在，執行修改操作會異常
                List<int> existsIds = App_NewsRepository.Instance.FindAsIQueryable(x => editIds.Contains(x.Id)).Select(s => s.Id).ToList();

                //獲取實際可以修改的數據
                List<App_News> updateList = newsList.Where(x => existsIds.Contains(x.Id)).ToList();

                //設置默認修改人信息
                updateList.ForEach(x => { x.SetModifyDefaultVal(); });
                //新增
                repository.AddRange(addList);
                //修改(第二個參數指定要修改的字段,第三個參數執行保存)
                repository.UpdateRange(updateList, x => new { x.Title, x.ImageUrl, x.Modifier, x.ModifyDate, x.ModifyID });
                //其他從表按上面同樣的操作即可
                //最終保存
                //repository.SaveChanges();
                return WebResponseContent.Instance.OK();
            };
            return base.Update(saveModel);
        }

        /// <summary>
        /// 刪除
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="delList"></param>
        /// <returns></returns>
        public override WebResponseContent Del(object[] keys, bool delList = true)
        {
            return base.Del(keys, true);
        }
    }

    public class TableExtra
    {
        /// <summary>
        /// 從表1
        /// </summary>
        public List<Table1> Table1List { get; set; }

        /// <summary>
        /// 從表2
        /// </summary>
        public List<Table2> Table2List { get; set; }
    }
    /// <summary>
    /// 從表1
    /// </summary>
    public class Table1
    {
        [Display(Name = "ID")]
        [Column(TypeName = "int")]
        public int? Id { get; set; }

        [Display(Name = "標題")]
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        public string Title { get; set; }

        [Display(Name = "封面图片")]
        [MaxLength(500)]
        [Column(TypeName = "nvarchar(500)")]
        public string ImageUrl { get; set; }

        [Display(Name = "内容推薦")]
        [Column(TypeName = "int")]
        public int? DailyRecommend { get; set; }

        [Display(Name = "是否啟用")]
        //   [Column(TypeName = "int")]
        public string Enable { get; set; }

        [Display(Name = "發佈時間")]
        [Column(TypeName = "datetime")]
        public DateTime? CreateDate { get; set; }
    }


    /// <summary>
    /// 從表1
    /// </summary>
    public class Table2
    {
        [Display(Name = "Id")]
        [MaxLength(36)]
        [Column(TypeName = "uniqueidentifier")]
        public Guid? Id { get; set; }

        [Display(Name = "姓名")]
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; }


        [Display(Name = "描述")]
        [MaxLength(250)]
        [Column(TypeName = "nvarchar(250)")]
        public string Describe { get; set; }

        [Display(Name = "電話")]
        [MaxLength(15)]
        [Column(TypeName = "nvarchar(15)")]
        public string PhoneNo { get; set; }

        [Display(Name = "創建時間")]
        [Column(TypeName = "datetime")]
        [Editable(true)]
        public DateTime? CreateDate { get; set; }
    }
}
