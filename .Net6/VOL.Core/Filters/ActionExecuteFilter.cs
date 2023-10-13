using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using VOL.Core.Enums;
using VOL.Core.Extensions;
using VOL.Core.ObjectActionValidator;
using VOL.Core.Services;
using VOL.Core.Utilities;

namespace VOL.Core.Filters
{
    public class ActionExecuteFilter : IActionFilter
    {

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //驗證方法參數
            context.ActionParamsValidator();
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}