using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;
using System.Security.Claims;
using VOL.Core.Configuration;
using VOL.Core.Extensions;
using VOL.Core.ManageUser;
using VOL.Core.Utilities;

namespace VOL.Core.Filters
{
    public interface IFixedTokenFilter : IFilterMetadata
    {
        AuthorizationFilterContext OnAuthorization(AuthorizationFilterContext context);
    }
    public class FixedTokenAttribute : Attribute, IFixedTokenFilter, IAllowAnonymous
    {
        public AuthorizationFilterContext OnAuthorization(AuthorizationFilterContext context)
        {
            string fixedoken = "";
            //如果token已失效，直接獲取header里的token
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                fixedoken = context.HttpContext.Request.Headers[AppSetting.TokenHeaderName];
                fixedoken = fixedoken?.Replace("Bearer ", "");
                //判斷是否傳入了token
                if (string.IsNullOrEmpty(fixedoken))
                {
                    return context.Unauthorized("沒有傳入token");
                }
                //解析token
                int userId = JwtHelper.GetUserId(fixedoken);
                if (userId <= 0)
                {
                    return context.Unauthorized("token不正確");
                }
                context.AddIdentity(userId);
            }
            else
            {
                fixedoken = ((ClaimsIdentity)context.HttpContext.User.Identity)
                ?.BootstrapContext?.ToString();
            }
            //判斷當前用戶的token與緩存的token是否相同
            if (UserContext.Current.Token != fixedoken)
            {
                context.FilterResult(HttpStatusCode.Unauthorized, "token已失效");
            }
            return context;
        }
    }
}
