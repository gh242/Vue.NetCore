/*
 *所有關於Sys_WorkFlowTableAuditLog類的業務代碼應在此處編寫
*可使用repository.調用常用方法，獲取EF/Dapper等信息
*如果需要事務請使用repository.DbContextBeginTransaction
*也可使用DBServerProvider.手動獲取數據庫相關信息
*用戶信息、權限、角色等使用UserContext.Current操作
*Sys_WorkFlowTableAuditLogService對增、刪、改查、導入、導出、審核業務代碼擴展參照ServiceFunFilter
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

namespace VOL.System.Services
{
    public partial class Sys_WorkFlowTableAuditLogService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISys_WorkFlowTableAuditLogRepository _repository;//訪問數據庫

        [ActivatorUtilitiesConstructor]
        public Sys_WorkFlowTableAuditLogService(
            ISys_WorkFlowTableAuditLogRepository dbRepository,
            IHttpContextAccessor httpContextAccessor
            )
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            //多租戶會用到這init代碼，其他情况可以不用
            //base.Init(dbRepository);
        }
  }
}
