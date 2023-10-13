/*
 *代碼由框架生成,任何更改都可能導致被代碼生成器覆蓋
 *如果要增加方法請在當前目錄下Partial文件夾App_TransactionAvgPriceController編寫
 */
using Microsoft.AspNetCore.Mvc;
using VOL.AppManager.IServices;
using VOL.Core.Controllers.Basic;
using VOL.Entity.AttributeManager;

namespace VOL.AppManager.Controllers
{
    [Route("api/App_TransactionAvgPrice")]
    [PermissionTable(Name = "App_TransactionAvgPrice")]
    public partial class App_TransactionAvgPriceController : ApiBaseController<IApp_TransactionAvgPriceService>
    {
        public App_TransactionAvgPriceController(IApp_TransactionAvgPriceService service)
        : base("AppManager","App","App_TransactionAvgPrice", service)
        {
        }
    }
}

