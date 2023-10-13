using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VOL.Core.Dapper;
using VOL.Core.EFDbContext;
using VOL.Core.Enums;
using VOL.Core.Utilities;
using VOL.Entity.SystemModels;

namespace VOL.Core.BaseProvider
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {

        /// <summary>
        /// EF DBContext
        /// </summary>
        VOLContext DbContext { get; }

        ISqlDapper DapperContext { get; }
        /// <summary>
        /// 執行事務。將在執行的方法帶入Action
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        WebResponseContent DbContextBeginTransaction(Func<WebResponseContent> action);

    
        /// <summary>
        /// 通過條件查詢數據
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        List<TEntity> Find(Expression<Func<TEntity, bool>> where);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderBySelector">排序字段,數據格式如:
        ///  orderBy = x => new Dictionary<object, bool>() {
        ///          { x.BalconyName,QueryOrderBy.Asc},
        ///          { x.TranCorpCode1,QueryOrderBy.Desc}
        ///         };
        /// 
        /// </param>
        /// <returns></returns>
        TEntity FindFirst(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, Dictionary<object, QueryOrderBy>>> orderBy = null);



        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate">where條件</param>
        /// <param name="orderBy">排序字段,數據格式如:
        ///  orderBy = x => new Dictionary<object, bool>() {
        ///          { x.BalconyName,QueryOrderBy.Asc},
        ///          { x.TranCorpCode1,QueryOrderBy.Desc}
        ///         };
        /// </param>
        /// <returns></returns>
        IQueryable<TEntity> FindAsIQueryable(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, Dictionary<object, QueryOrderBy>>> orderBy = null);
        /// <summary>
        /// 通過條件查詢數據
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">查詢條件</param>
        /// <param name="selector">返回類型如:Find(x => x.UserName == loginInfo.userName, p => new { uname = p.UserName });</param>
        /// <returns></returns>
        List<T> Find<T>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, T>> selector);



        /// <summary>
        /// 根據條件，返回查詢的類
        /// </summary>
        /// <typeparam name="TFind"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        List<TFind> Find<TFind>(Expression<Func<TFind, bool>> predicate) where TFind : class;

        Task<TFind> FindAsyncFirst<TFind>(Expression<Func<TFind, bool>> predicate) where TFind : class;

        Task<TEntity> FindAsyncFirst(Expression<Func<TEntity, bool>> predicate);

        Task<List<TFind>> FindAsync<TFind>(Expression<Func<TFind, bool>> predicate) where TFind : class;
        Task<TEntity> FindFirstAsync(Expression<Func<TEntity, bool>> predicate);

        Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

        Task<List<T>> FindAsync<T>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, T>> selector);

        Task<T> FindFirstAsync<T>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, T>> selector);

        /// <summary>
        /// 多條件查詢
        /// </summary>
        /// <typeparam name="Source"></typeparam>
        /// <param name="sources">要查詢的多個條件的數據源</param>
        /// <param name="predicate">生成的查詢條件</param>
        /// <returns></returns>
        List<TEntity> Find<Source>(IEnumerable<Source> sources,
            Func<Source, Expression<Func<TEntity, bool>>> predicate)
            where Source : class;
        /// <summary>
        /// 多條件查詢
        /// </summary>
        /// <typeparam name="Source"></typeparam>
        /// <param name="sources">要查詢的多個條件的數據源</param>
        /// <param name="predicate">生成的查詢條件</param>
        /// <param name="selector">自定義返回結果</param>
        /// <returns></returns>
        List<TResult> Find<Source, TResult>(IEnumerable<Source> sources,
            Func<Source, Expression<Func<TEntity, bool>>> predicate,
            Expression<Func<TEntity, TResult>> selector)
            where Source : class;

        /// <summary>
        /// 多條件查詢
        /// </summary>
        /// <typeparam name="Source"></typeparam>
        /// <param name="sources">要查詢的多個條件的數據源</param>
        /// <param name="predicate">生成的查詢條件</param>
        /// <returns></returns>
        IQueryable<TEntity> FindAsIQueryable<Source>(IEnumerable<Source> sources,
            Func<Source, Expression<Func<TEntity, bool>>> predicate)
            where Source : class;

        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);

        bool Exists(Expression<Func<TEntity, bool>> predicate);

        bool Exists<TExists>(Expression<Func<TExists, bool>> predicate) where TExists : class;

        Task<bool> ExistsAsync<TExists>(Expression<Func<TExists, bool>> predicate) where TExists : class;

        IIncludableQueryable<TEntity, TProperty> Include<TProperty>(Expression<Func<TEntity, TProperty>> incluedProperty);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pagesize"></param>
        /// <param name="rowcount"></param>
        /// <param name="predicate"></param>
        /// <param name="orderBy">
        /// 通過多個字段排序Expression<Func<TEntity, Dictionary<object, bool>>>
        ///  orderBy = x => new Dictionary<object, bool>() {
        ///          { x.BalconyName,QueryOrderBy.Asc},
        ///          { x.TranCorpCode1,QueryOrderBy.Desc}
        ///         };
        /// <param name="selectorResult">查詢返回的對象</param>
        /// <returns></returns>
        List<TResult> QueryByPage<TResult>(int pageIndex, int pagesize, out int rowcount, Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, Dictionary<object, QueryOrderBy>>> orderBySelector, Expression<Func<TEntity, TResult>> selectorResult, bool returnRowCount = true);

        List<TResult> QueryByPage<TResult>(int pageIndex, int pagesize, Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, Dictionary<object, QueryOrderBy>>> orderBy, Expression<Func<TEntity, TResult>> selectorResult = null);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pagesize"></param>
        /// <param name="rowcount"></param>
        /// <param name="predicate"></param>
        /// <param name="orderBy"></param>
        ///         /// 通過多個字段排序Expression<Func<TEntity, Dictionary<object, bool>>>
        ///  orderBy = x => new Dictionary<object, bool>() {
        ///          { x.BalconyName,QueryOrderBy.Asc},
        ///          { x.TranCorpCode1,QueryOrderBy.Desc}
        ///         };
        /// <returns></returns>
        List<TEntity> QueryByPage(int pageIndex, int pagesize, out int rowcount, Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, Dictionary<object, QueryOrderBy>>> orderBy, bool returnRowCount = true);

        IQueryable<TFind> IQueryablePage<TFind>(int pageIndex, int pagesize, out int rowcount, Expression<Func<TFind, bool>> predicate, Expression<Func<TEntity, Dictionary<object, QueryOrderBy>>> orderBy, bool returnRowCount = true) where TFind : class;


        IQueryable<TEntity> IQueryablePage(IQueryable<TEntity> queryable, int pageIndex, int pagesize, out int rowcount, Dictionary<string, QueryOrderBy> orderBy, bool returnRowCount = true);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="properties">指定更新字段:x=>new {x.Name,x.Enable}</param>
        /// <param name="saveChanges">是否保存</param>
        /// <returns></returns>

        int Update(TEntity entity, Expression<Func<TEntity, object>> properties, bool saveChanges = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="properties">指定更新字段:x=>new {x.Name,x.Enable}</param>
        /// <param name="saveChanges">是否保存</param>
        /// <returns></returns>
        int Update<TSource>(TSource entity, Expression<Func<TSource, object>> properties, bool saveChanges = false) where TSource : class;

        int Update<TSource>(TSource entity, bool saveChanges = false) where TSource : class;

        int Update<TSource>(TSource entity, string[] properties, bool saveChanges = false) where TSource : class;

        int UpdateRange<TSource>(IEnumerable<TSource> entities, bool saveChanges = false) where TSource : class;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="properties">指定更新字段:x=>new {x.Name,x.Enable}</param>
        /// <param name="saveChanges">是否保存</param>
        /// <returns></returns>
        int UpdateRange<TSource>(IEnumerable<TSource> models, Expression<Func<TSource, object>> properties, bool saveChanges = false) where TSource : class;

        int UpdateRange<TSource>(IEnumerable<TSource> entities, string[] properties, bool saveChanges = false) where TSource : class;


        /// <summary>
        ///修改時同時對明細的添加、刪除、修改
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="updateDetail">是否修改明細</param>
        /// <param name="delNotExist">是否刪除明細不存在的數據</param>
        /// <param name="updateMainFields">主表指定修改字段</param>
        /// <param name="updateDetailFields">明細指定修改字段</param>
        /// <param name="saveChange">是否保存</param>
        /// <returns></returns>
        WebResponseContent UpdateRange<Detail>(TEntity entity,
            bool updateDetail = false,
            bool delNotExist = false,
            Expression<Func<TEntity, object>> updateMainFields = null,
            Expression<Func<Detail, object>> updateDetailFields = null,
            bool saveChange = false) where Detail : class;

        void Delete(TEntity model, bool saveChanges=false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="delList">是否將子表的數據也刪除</param>
        /// <returns></returns>
        int DeleteWithKeys(object[] keys, bool delList = false);

        void Add(TEntity entities, bool SaveChanges = false);
        void AddRange(IEnumerable<TEntity> entities, bool SaveChanges = false);

        Task AddAsync(TEntity entities);
        Task AddRangeAsync(IEnumerable<TEntity> entities);

        void AddRange<T>(IEnumerable<T> entities, bool saveChanges = false)
           where T : class;


       void BulkInsert(IEnumerable<TEntity> entities, bool setOutputIdentity = false);

        int SaveChanges();

        Task<int> SaveChangesAsync();



        int ExecuteSqlCommand(string sql, params SqlParameter[] sqlParameters);

        List<TEntity> FromSql(string sql, params SqlParameter[] sqlParameters);

        /// <summary>
        /// 執行sql
        /// 使用方式 FormattableString sql=$"select * from xx where name ={xx} and pwd={xx1} "，
        /// FromSqlInterpolated内部處理sql注入的問題，直接在{xx}寫對應的值即可
        /// 注意：sql必須 select * 返回所有TEntity字段，
        /// </summary>
        /// <param name="formattableString"></param>
        /// <returns></returns>
        IQueryable<TEntity> FromSqlInterpolated([System.Diagnostics.CodeAnalysis.NotNull] FormattableString sql);


        /// <summary>
        /// 取消上下文跟踪(2021.08.22)
        /// 更新報錯時，請調用此方法：The instance of entity type 'XXX' cannot be tracked because another instance with the same key value for {'XX'} is already being tracked.
        /// </summary>
        /// <param name="entity"></param>
        void Detached(TEntity entity);
        void DetachedRange(IEnumerable<TEntity> entities);



        IQueryable<TEntity> WhereIF([NotNull] Expression<Func<TEntity, object>> field, string value, LinqExpressionType linqExpression = LinqExpressionType.Equal);

        /// <summary>
        ///  if判斷查詢
        /// </summary>
        /// 查詢示例，value不為null時參與條件查詢
        ///    string value = null;
        ///    repository.WhereIF(value!=null,x=>x.Creator==value);
        /// <param name="checkCondition"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IQueryable<TEntity> WhereIF(bool checkCondition, Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        ///  if判斷查詢
        /// </summary>
        /// 查詢示例，value不為null時參與條件查詢
        ///    string value = null;
        ///    repository.WhereIF<Sys_User>(value!=null,x=>x.Creator==value);
        /// <param name="checkCondition"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IQueryable<T> WhereIF<T>(bool checkCondition, Expression<Func<T, bool>> predicate) where T : class;
    }
}
