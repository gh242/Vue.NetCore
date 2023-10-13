using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace VOL.Core.Middleware
{
    public class HttpRequestMiddleware
    {
        public static Func<RequestDelegate, RequestDelegate> Context
        {
            get
            {
                return next => async context =>
                {
                    //動態標識刷新token(2021.05.01)
                    context.Response.Headers.Add("Access-Control-Expose-Headers", "vol_exp");
                    await next(context);
                };

            }
        }
    }
   
}
