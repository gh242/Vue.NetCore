﻿//using Aliyun.Acs.Core;
//using Aliyun.Acs.Core.Auth.Sts;
//using Aliyun.Acs.Core.Exceptions;
//using Aliyun.Acs.Core.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VOL.Core.CacheManager;
using VOL.Core.Filters;
using VOL.Core.Services;
using VOL.Core.Utilities;

namespace VOL.WebApi.Controllers.OSS
{
    /// <summary>
    /// neuget包aliyun-net-sdk-core
    /// </summary>
    [JWTAuthorize, ApiController]
    [Route("api/alioss")]
    public class AliOSSController : Controller
    {
        private ICacheService _cache { get; set; }
        public AliOSSController(ICacheService cache)
        {
            _cache = cache;
        }
    
        [Route("getAccessToken"), HttpGet]
        public object GetAccessToken()
        {

            // //Region對照(創建創建 Bucket時選擇的地域)https://oss.console.aliyun.com/bucket列表中的【地域】列，根據地域在下面的連接裡面【Region ID】值 
            // //https://help.aliyun.com/document_detail/31837.htm?spm=a2c4g.11186623.0.0.57a8396cwRnyQV#section-plb-2vy-5db
            // string region = "oss-cn-beijing";

             
            ////https://ram.console.aliyun.com/users 用戶點擊進去裡面找
            // string accessKeyID = "LTAI5tR4bQnBZqF8ruGiw123";
            // string accessKeySecret = "gZUwXdy1mVsPvROfNCsvmVWOeqi123";

            //// https://ram.console.aliyun.com/roles/AliyunServiceRoleForSLSAudit 角色點進去找ARN
            // string ARN = "acs:ram::1807122303681234:role/vol-role";
            // WebResponseContent webResponse = new WebResponseContent();

            // //下面這些引用的neuget包aliyun-net-sdk-core

            // //獲取sts憑證
            // IClientProfile profile = DefaultProfile.GetProfile(region, accessKeyID, accessKeySecret);
            // DefaultAcsClient client = new DefaultAcsClient(profile);
            // var request = new AssumeRoleRequest()
            // {
            //      RoleArn = ARN, 
            //     //這個 随便填寫
            //     RoleSessionName = "oss"
            // };
            // try
            // {
            //     var response = client.GetAcsResponse(request);

            //     return Json(webResponse.OK(null, new
            //     {
            //         region,
            //         response.Credentials.AccessKeyId,
            //         response.Credentials.AccessKeySecret,
            //         response.Credentials.SecurityToken,
            //         Bucket= "vol-2023",//阿里雲控制台創建的bucket名稱
            //         //Bucket所在具體文件夾
            //         BucketFolder = "/" + DateTime.Now.ToString("yyyyMMdd"),
            //         //生成一個唯一標識防止文件重覆
            //         unique = DateTime.Now.ToString("HHmmsss")
            //     }));

            // }
            // catch (ServerException e)
            // {
            //     string message = $"獲取sts異常：{e.Message + e.StackTrace}";
            //     Console.WriteLine(message);
            //     Logger.Error(message);
            //     return Json(webResponse.Error(message));
            // }
            return Content("");
        }
    }
}
