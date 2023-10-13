/*
 *接口編寫處...
*如果接口需要做Action的權限驗證，請在Action上使用屬性
*如: [ApiActionPermission("Sys_WorkFlowTableAuditLog",Enums.ActionPermissionOptions.Search)]
 */
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using VOL.Entity.DomainModels;
using VOL.System.IServices;

namespace VOL.System.Controllers
{
    public partial class Sys_WorkFlowTableAuditLogController
    {
        private readonly ISys_WorkFlowTableAuditLogService _service;//訪問業務代碼
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Sys_WorkFlowTableAuditLogController(
            ISys_WorkFlowTableAuditLogService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
