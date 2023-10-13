/*
 *代碼由框架生成,任何更改都可能導致被代碼生成器覆蓋
 *如果要增加方法請在當前目錄下Partial文件夾Demo_CatalogController編寫
 */
using Microsoft.AspNetCore.Mvc;
using VOL.Core.Controllers.Basic;
using VOL.Entity.AttributeManager;
using VOL.Demo.IServices;
namespace VOL.Demo.Controllers
{
    [Route("api/Demo_Catalog")]
    [PermissionTable(Name = "Demo_Catalog")]
    public partial class Demo_CatalogController : ApiBaseController<IDemo_CatalogService>
    {
        public Demo_CatalogController(IDemo_CatalogService service)
        : base(service)
        {
        }
    }
}

