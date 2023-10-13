/*
 *所有關於Demo_Order類的業務代碼應在此處編寫
*可使用repository.調用常用方法，獲取EF/Dapper等信息
*如果需要事務請使用repository.DbContextBeginTransaction
*也可使用DBServerProvider.手動獲取數據庫相關信息
*用戶信息、權限、角色等使用UserContext.Current操作
*Demo_OrderService對增、刪、改查、導入、導出、審核業務代碼擴展參照ServiceFunFilter
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
using Microsoft.AspNetCore.Mvc;
using System.Net;
using VOL.Core.Filters;
using VOL.Core.Configuration;

namespace VOL.Demo.Services
{
    public partial class Demo_OrderService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDemo_OrderRepository _repository;//訪問數據庫

        [ActivatorUtilitiesConstructor]
        public Demo_OrderService(
            IDemo_OrderRepository dbRepository,
            IHttpContextAccessor httpContextAccessor
        )
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            //多租戶會用到這init代碼，其他情况可以不用
            //base.Init(dbRepository);
        }

        public override PageGridData<Demo_Order> GetPageData(PageDataOptions options)
        {
            //AppSetting.ExpMinutes

            //2020.08.15
            //此處與上面QuerySql，只需要實現其中一個就可以了
            //查詢前可以自己設定查詢表達式的條件
            QueryRelativeExpression = (IQueryable<Demo_Order> queryable) =>
            {
                //當前用戶只能操作自己(與下級角色)創建的數據，如：查詢、刪除、修改等操作
                //IQueryable<int> userQuery = RoleContext.GetCurrentAllChildUser();
                //queryable = queryable.Where(x => x.CreateID == UserContext.Current.UserId || userQuery.Contains(x.CreateID ?? 0));
                return queryable;
            };

            return base.GetPageData(options);
        }
    }
}
