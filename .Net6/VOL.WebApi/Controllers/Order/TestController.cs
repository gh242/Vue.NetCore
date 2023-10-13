using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VOL.WebApi.Controllers.Order
{
    /// <summary>
    /// 測試swagger對控制器的注釋描述
    /// </summary>
    [ApiExplorerSettings(GroupName = "v2")]//Select a definition 下拉選項
    public class TestController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
