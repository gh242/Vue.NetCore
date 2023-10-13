﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VOL.Core.Configuration;
using VOL.Core.Extensions;
using VOL.Core.Filters;
using VOL.Core.Middleware;
using VOL.Core.Services;
using VOL.Core.Utilities;
using VOL.Entity.DomainModels;

namespace VOL.Core.Controllers.Basic
{
    [JWTAuthorize, ApiController]
    public class ApiBaseController<IServiceBase> : VolController
    {
        protected IServiceBase Service;
        private WebResponseContent _baseWebResponseContent { get; set; }
        public ApiBaseController()
        {
        }
        public ApiBaseController(IServiceBase service)
        {
            Service = service;
        }
        public ApiBaseController(string projectName, string folder, string tablename, IServiceBase service)
        {
            Service = service;
        }
        [ActionLog("查詢")]
        [ApiActionPermission(Enums.ActionPermissionOptions.Search)]
        [HttpPost, Route("GetPageData")]
        public virtual ActionResult GetPageData([FromBody] PageDataOptions loadData)
        {
            return JsonNormal(InvokeService("GetPageData", new object[] { loadData }));
        }

        /// <summary>
        /// 獲取明細grid分頁數據
        /// </summary>
        /// <param name="loadData"></param>
        /// <returns></returns>
        [ActionLog("明細查詢")]
        [ApiActionPermission(Enums.ActionPermissionOptions.Search)]
        [HttpPost, Route("GetDetailPage")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual ActionResult GetDetailPage([FromBody] PageDataOptions loadData)
        {
            return Content(InvokeService("GetDetailPage", new object[] { loadData }).Serialize());
        }

        /// <summary>
        /// 上傳文件
        /// </summary>
        /// <param name="fileInput"></param>
        /// <returns></returns>
        [ActionLog("上傳文件")]
        [HttpPost, Route("Upload")]
        [ApiActionPermission(Enums.ActionPermissionOptions.Upload)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual IActionResult Upload(IEnumerable<IFormFile> fileInput)
        {
            return Json(InvokeService("Upload", new object[] { fileInput }));
        }
        /// <summary>
        /// 下載導入Excel模板
        /// </summary>
        /// <returns></returns>
        [ActionLog("下載導入Excel模板")]
        [HttpGet, Route("DownLoadTemplate")]
        [ApiActionPermission(Enums.ActionPermissionOptions.Import)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual ActionResult DownLoadTemplate()
        {
            _baseWebResponseContent = InvokeService("DownLoadTemplate", new object[] { }) as WebResponseContent;
            if (!_baseWebResponseContent.Status) return Json(_baseWebResponseContent);
            byte[] fileBytes = System.IO.File.ReadAllBytes(_baseWebResponseContent.Data.ToString());
            return File(
                    fileBytes,
                    System.Net.Mime.MediaTypeNames.Application.Octet,
                    Path.GetFileName(_baseWebResponseContent.Data.ToString())
                );
        }
        /// <summary>
        /// 導入表數據Excel
        /// </summary>
        /// <param name="fileInput"></param>
        /// <returns></returns>
        [ActionLog("導入Excel")]
        [HttpPost, Route("Import")]
        [ApiActionPermission(Enums.ActionPermissionOptions.Import)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual ActionResult Import(List<IFormFile> fileInput)
        {
            return Json(InvokeService("Import", new object[] { fileInput }));
        }

        /// <summary>
        /// 導出文件，返回日期+文件名
        /// </summary>
        /// <param name="loadData"></param>
        /// <returns></returns>
        [ActionLog("導出Excel")]
        [ApiActionPermission(Enums.ActionPermissionOptions.Export)]
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost, Route("Export")]
        public virtual ActionResult Export([FromBody] PageDataOptions loadData)
        {
            var result = InvokeService("Export", new object[] { loadData }) as WebResponseContent;
            return File(
                   System.IO.File.ReadAllBytes(result.Data.ToString().MapPath()),
                   System.Net.Mime.MediaTypeNames.Application.Octet,
                   Path.GetFileName(result.Data.ToString())
               );
        }


        /// <summary>
        /// 通過key刪除文件
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
       // [ActionLog("刪除")]
        [ApiActionPermission(Enums.ActionPermissionOptions.Delete)]
        [HttpPost, Route("Del")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual ActionResult Del([FromBody] object[] keys)
        {
            _baseWebResponseContent = InvokeService("Del", new object[] { keys, true }) as WebResponseContent;
            Logger.Info(Enums.LoggerType.Del, keys.Serialize(), _baseWebResponseContent.Status ? "Ok" : _baseWebResponseContent.Message);
            return Json(_baseWebResponseContent);
        }
        /// <summary>
        /// 審核
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        /// [ActionLog("審核")]
        [ApiActionPermission(Enums.ActionPermissionOptions.Audit)]
        [HttpPost, Route("Audit")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual ActionResult Audit([FromBody] object[] id, int? auditStatus, string auditReason)
        {
            _baseWebResponseContent = InvokeService("Audit", new object[] { id, auditStatus, auditReason }) as WebResponseContent;
            Logger.Info(Enums.LoggerType.Del, id?.Serialize() + "," + (auditStatus ?? -1) + "," + auditReason, _baseWebResponseContent.Status ? "Ok" : _baseWebResponseContent.Message);
            return Json(_baseWebResponseContent);
        }
        /// <summary>
        /// 新增支持主子表
        /// </summary>
        /// <param name="saveDataModel"></param>
        /// <returns></returns>
        [ActionLog("新建")]
        [ApiActionPermission(Enums.ActionPermissionOptions.Add)]
        [HttpPost, Route("Add")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual ActionResult Add([FromBody] SaveModel saveModel)
        {
            _baseWebResponseContent = InvokeService("Add",
                new Type[] { typeof(SaveModel) },
                new object[] { saveModel }) as WebResponseContent;
            Logger.Info(Enums.LoggerType.Add, null, _baseWebResponseContent.Status ? "Ok" : _baseWebResponseContent.Message);
            _baseWebResponseContent.Data = _baseWebResponseContent.Data?.Serialize();
            return Json(_baseWebResponseContent);
        }
        /// <summary>
        /// 編輯支持主子表
        /// [ModelBinder(BinderType =(typeof(ModelBinder.BaseModelBinder)))]可指定綁定modelbinder
        /// </summary>
        /// <param name="saveDataModel"></param>
        /// <returns></returns>
        [ActionLog("編輯")]
        [ApiActionPermission(Enums.ActionPermissionOptions.Update)]
        [HttpPost, Route("Update")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual ActionResult Update([FromBody] SaveModel saveModel)
        {
            _baseWebResponseContent = InvokeService("Update", new object[] { saveModel }) as WebResponseContent;
            Logger.Info(Enums.LoggerType.Edit, null, _baseWebResponseContent.Status ? "Ok" : _baseWebResponseContent.Message);
            _baseWebResponseContent.Data = _baseWebResponseContent.Data?.Serialize();
            return Json(_baseWebResponseContent);
        }

        /// <summary>
        /// 調用service方法
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private object InvokeService(string methodName, object[] parameters)
        {
            return Service.GetType().GetMethod(methodName).Invoke(Service, parameters);
        }
        /// <summary>
        /// 調用service方法
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="types">為要調用重載的方法參數類型：new Type[] { typeof(SaveDataModel)</param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private object InvokeService(string methodName, Type[] types, object[] parameters)
        {
            return Service.GetType().GetMethod(methodName, types).Invoke(Service, parameters);
        }
    }
}
