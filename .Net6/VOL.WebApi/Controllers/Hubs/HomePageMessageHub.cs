using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOL.Core.CacheManager;
using VOL.Core.Extensions;
using VOL.Core.ManageUser;
using VOL.System.IServices;

namespace VOL.WebApi.Controllers.Hubs
{
    /// <summary>
    /// https://docs.microsoft.com/zh-cn/aspnet/core/signalr/introduction?view=aspnetcore-3.1
    /// https://docs.microsoft.com/zh-cn/aspnet/core/signalr/javascript-client?view=aspnetcore-6.0&tabs=visual-studio
    /// </summary>
    public class HomePageMessageHub : Hub
    {
        private readonly ICacheService _cacheService;


        private static ConcurrentDictionary<string, string> _connectionIds = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// 構造 注入
        /// </summary>
        public HomePageMessageHub(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        /// <summary>
        /// 建立連接時異步觸發
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            //Console.WriteLine($"建立連接{Context.ConnectionId}");
            _connectionIds[Context.ConnectionId] = Context.GetHttpContext().Request.Query["userName"].ToString();
            //添加到一個組下
            //await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
            //發送上線消息
            //await Clients.All.SendAsync("ReceiveHomePageMessage", 1, new { title = "系統消息", content = $"{Context.ConnectionId} 上線" });
            await base.OnConnectedAsync();
        }

        /// <summary>
        /// 離開連接時異步觸發
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            //Console.WriteLine($"斷開連接{Context.ConnectionId}");
            //從組中刪除
            // await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
            //可自行調用下線業務處理方法...
            await UserOffline();
            //發送下線消息
            //   await Clients.All.SendAsync("ReceiveHomePageMessage", 4, new { title = "系統消息", content = $"{Context.ConnectionId} 離線" });
            await base.OnDisconnectedAsync(ex);
        }

        /// <summary>
        /// 根據用戶名獲取所有的客戶端
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private IEnumerable<string> GetCnnectionIds(string username)
        {
            foreach (var item in _connectionIds)
            {
                if (item.Value == username)
                {
                    yield return item.Key;
                }
            }
        }

        /// <summary>
        /// 發送給指定的人
        /// </summary>
        /// <param name="username">sys_user表的登入帳號</param>
        /// <param name="message">發送的消息</param>
        /// <returns></returns>
        public async Task<bool> SendHomeMessage(string username, string title, string message)
        {
            if (_connectionIds[Context.ConnectionId]!="admin")
            {
                return false;
            }
            await Clients.Clients(GetCnnectionIds(username).ToArray()).SendAsync("ReceiveHomePageMessage", new
            {
                //   username,
                title,
                message,
                date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss")
            });
            return true;
        }

        /// <summary>
        /// 斷開連接
        /// </summary>
        /// <returns></returns>
        public async Task<bool> UserOffline()
        {
            var cid = Context.ConnectionId;//也可以從緩存中獲取ConnectionId
            //  await Clients.Client(cid).SendAsync("ReceiveHomePageMessage", 3, new { title = "系統消息", content = "離線成功" });
            //移除緩存
            if (_connectionIds.TryRemove(cid, out string value))
            {
            }
            await Task.CompletedTask;
            return true;
        }


    }
}
