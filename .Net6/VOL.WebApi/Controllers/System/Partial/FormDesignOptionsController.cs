/*
 *接口編寫處...
*如果接口需要做Action的權限驗證，請在Action上使用屬性
*如: [ApiActionPermission("FormDesignOptions",Enums.ActionPermissionOptions.Search)]
 */
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using VOL.Entity.DomainModels;
using VOL.System.IServices;
using VOL.System.IRepositories;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using VOL.System.Services;

namespace VOL.System.Controllers
{
    public partial class FormDesignOptionsController
    { 
        private readonly IFormDesignOptionsService _service;//訪問業務代碼
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFormCollectionObjectRepository _formCollectionRepository;
        private readonly IFormDesignOptionsRepository _formDesignOptionsRepository;
        [ActivatorUtilitiesConstructor]
        public FormDesignOptionsController(
            IFormDesignOptionsService service,
            IHttpContextAccessor httpContextAccessor,
            IFormCollectionObjectRepository formCollectionRepository,
            IFormDesignOptionsRepository formDesignOptionsRepository
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
            _formCollectionRepository = formCollectionRepository;
            _formDesignOptionsRepository = formDesignOptionsRepository;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("getFormOptions"), HttpGet]
        public async Task<IActionResult> GetFormOptions(Guid id)
        {
            var options = await _formDesignOptionsRepository.FindAsIQueryable(x => x.FormId == id)
                    .Select(s => new { s.Title, s.FormOptions })  
                    .FirstOrDefaultAsync();
            return Json(new { data = options });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="saveModel"></param>
        /// <returns></returns>
        [Route("submit"), HttpPost]
        public IActionResult Submit([FromBody] SaveModel saveModel)
        {
            var result = FormCollectionObjectService.Instance.Add(saveModel);
            return Json(result);
        }
        /// <summary>
        ///獲取有數據的設計器
        /// </summary>
        /// <returns></returns>
        [Route("getList"), HttpGet]
        public IActionResult GetList()
        {
            var query = _formCollectionRepository.FindAsIQueryable(x => true);
            var data = _formDesignOptionsRepository.FindAsIQueryable(x => query.Any(c => c.FormId == x.FormId))
                  .Select(s => new { s.FormId, s.Title, s.FormOptions })
                  .ToList();  
            return Json(data);  

        }
    }
}
