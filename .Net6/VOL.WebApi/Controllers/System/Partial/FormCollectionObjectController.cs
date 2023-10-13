/*
 *接口編寫處...
*如果接口需要做Action的權限驗證，請在Action上使用屬性
*如: [ApiActionPermission("FormCollectionObject",Enums.ActionPermissionOptions.Search)]
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
    public partial class FormCollectionObjectController
    {
        private readonly IFormCollectionObjectService _service;//訪問業務代碼
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public FormCollectionObjectController( 
            IFormCollectionObjectService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
