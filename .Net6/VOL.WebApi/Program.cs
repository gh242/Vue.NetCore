using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VOL.Core.Configuration;

using VOL.WebApi.Controllers.MqDataHandle;

namespace VOL.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
            //CreateHostBuilder(args).Build().Run();
            var host = CreateHostBuilder(args).Build();
            #region kafka訂閱消息
            //if (AppSetting.Kafka.UseConsumer)
            //{
            //    using var scope = host.Services.CreateScope();
            //    var testConsumer = scope.ServiceProvider.GetService<IKafkaConsumer<string, string>>();
            //    testConsumer.Consume(res =>
            //    {
            //        Console.WriteLine($"recieve:{DateTime.Now.ToLongTimeString()}  value:{res.Message.Value}");
            //        //靜態方法 數據處理 入庫等操作
            //        bool bl = DataHandle.AlarmData(res.Message.Value);
            //        //回調函數需返回便於執行commit
            //        return bl;
            //    }, AppSetting.Kafka.Topics.TestTopic);
            //}
            #endregion
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
               Host.CreateDefaultBuilder(args)
                   .ConfigureWebHostDefaults(webBuilder =>
                   {
                       webBuilder.ConfigureKestrel(serverOptions =>
                       {
                           serverOptions.Limits.MaxRequestBodySize = 10485760;
                           // Set properties and call methods on options
                       });
                       webBuilder.UseKestrel().UseUrls("http://*:9991");
                       webBuilder.UseIIS();
                       webBuilder.UseStartup<Startup>();
                   }).UseServiceProviderFactory(new AutofacServiceProviderFactory());
    }
}
