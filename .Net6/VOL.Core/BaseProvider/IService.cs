
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VOL.Core.CacheManager;
using VOL.Core.Utilities;
using VOL.Entity.DomainModels;
using VOL.Entity.SystemModels;

namespace VOL.Core.BaseProvider
{
    public interface IService<T> where T : BaseEntity
    {

        ICacheService CacheContext { get; }
        Microsoft.AspNetCore.Http.HttpContext Context { get; }

        /// <summary>
        /// 前端查詢條件轉換為EF查詢Queryable(2023.04.02)
        /// </summary>
        /// <param name="options">前端查詢參數</param>
        /// <param name="useTenancy">是否使用數據隔離</param>
        /// <returns></returns>
        IQueryable<T> GetPageDataQueryFilter(PageDataOptions options, bool useTenancy = true);
        /// <summary>
        /// 查詢
        /// </summary>
        /// <param name="pageData"></param>
        /// <returns></returns>
        PageGridData<T> GetPageData(PageDataOptions pageData);

        object GetDetailPage(PageDataOptions pageData);

        WebResponseContent Upload(List<IFormFile> files);

        WebResponseContent DownLoadTemplate();

        WebResponseContent Import(List<IFormFile> files);
        /// <summary>
        /// 導出
        /// </summary>
        /// <param name="pageData"></param>
        /// <returns></returns>
        WebResponseContent Export(PageDataOptions pageData);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDataModel">主表與子表的數據</param>
        /// <returns></returns>
        WebResponseContent Add(SaveModel saveDataModel);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity">保存的實體</param>
        /// <param name="validationEntity">是否對實體進行校驗</param>
        /// <returns></returns>
        WebResponseContent AddEntity(T entity, bool validationEntity = true);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TDetail"></typeparam>
        /// <param name="entity">保存的實體</param>
        /// <param name="list">保存的明細</param>
        /// <param name="validationEntity">是否對實體進行校驗</param>
        /// <returns></returns>
        WebResponseContent Add<TDetail>(T entity, List<TDetail> list = null, bool validationEntity = true) where TDetail : class;
        /// <summary>
        /// 編輯
        /// </summary>
        /// <param name="saveDataModel">主表與子表的數據</param>
        /// <returns></returns>
        WebResponseContent Update(SaveModel saveDataModel);


        /// <summary>
        /// 刪除數據
        /// </summary>
        /// <param name="keys">刪除的主鍵</param>
        /// <param name="delList">是否刪除對應明細(默認會刪除明細)</param>
        /// <returns></returns>
        WebResponseContent Del(object[] keys, bool delList = true);

        WebResponseContent Audit(object[] id, int? auditStatus, string auditReason);


        (string, T, bool) ApiValidate(string bizContent, Expression<Func<T, object>> expression = null);


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <param name="bizContent"></param>
        /// <param name="expression">對指屬性驗證格式如：x=>new { x.UserName,x.Value }</param>
        /// <returns>(string,TInput, bool) string:返回驗證消息,TInput：bizContent序列化後的對象,bool:驗證是否通過</returns>
        (string, TInput, bool) ApiValidateInput<TInput>(string bizContent, Expression<Func<TInput, object>> expression);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <param name="bizContent"></param>
        /// <param name="expression">對指屬性驗證格式如：x=>new { x.UserName,x.Value }</param>
        /// <param name="validateExpression">對指定的字段只做合法性判斷比如長度是是否超長</param>
        /// <returns>(string,TInput, bool) string:返回驗證消息,TInput：bizContent序列化後的對象,bool:驗證是否通過</returns>
        (string, TInput, bool) ApiValidateInput<TInput>(string bizContent, Expression<Func<TInput, object>> expression, Expression<Func<TInput, object>> validateExpression);


        /// <summary>
        /// 將數據源映射到新的數據中,List<TSource>映射到List<TResult>或TSource映射到TResult
        /// 目前只支持Dictionary或實體類型
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="resultExpression">只映射返回對象的指定字段</param>
        /// <param name="sourceExpression">只映射數據源對象的指定字段</param>
        /// 過濾條件表達式調用方式：List表達式x => new { x[0].MenuName, x[0].Menu_Id}，表示指定映射MenuName,Menu_Id字段
        ///  List<Sys_Menu> list = new List<Sys_Menu>();
        ///  list.MapToObject<List<Sys_Menu>, List<Sys_Menu>>(x => new { x[0].MenuName, x[0].Menu_Id}, null);
        ///  
        ///過濾條件表達式調用方式：實體表達式x => new { x.MenuName, x.Menu_Id}，表示指定映射MenuName,Menu_Id字段
        ///  Sys_Menu sysMenu = new Sys_Menu();
        ///  sysMenu.MapToObject<Sys_Menu, Sys_Menu>(x => new { x.MenuName, x.Menu_Id}, null);
        /// <returns></returns>
        TResult MapToEntity<TSource, TResult>(TSource source, Expression<Func<TResult, object>> resultExpression,
           Expression<Func<TSource, object>> sourceExpression = null) where TResult : class;

        /// <summary>
        /// 將一個實體的賦到另一個實體上,應用場景：
        /// 兩個實體，a a1= new a();b b1= new b();  a1.P=b1.P; a1.Name=b1.Name;
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="result"></param>
        /// <param name="expression">指定對需要的字段賦值,格式x=>new {x.Name,x.P},返回的結果只會對Name與P賦值</param>
        void MapValueToEntity<TSource, TResult>(TSource source, TResult result, Expression<Func<TResult, object>> expression = null) where TResult : class;


    }
}
