/*
 *代碼由框架生成,任何更改都可能導致被代碼生成器覆蓋
 *如果要增加方法請在當前目錄下Partial文件夾Demo_GoodsController編寫
 */
using Microsoft.AspNetCore.Mvc;
using VOL.Core.Controllers.Basic;
using VOL.Entity.AttributeManager;
using VOL.Demo.IServices;
namespace VOL.Demo.Controllers
{
    [Route("api/Demo_Goods")]
    [PermissionTable(Name = "Demo_Goods")]
    public partial class Demo_GoodsController : ApiBaseController<IDemo_GoodsService>
    {
        public Demo_GoodsController(IDemo_GoodsService service)
        : base(service)
        {
        }
    }
}

