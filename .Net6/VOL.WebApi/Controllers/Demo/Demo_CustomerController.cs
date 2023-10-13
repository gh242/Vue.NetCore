/*
 *代碼由框架生成,任何更改都可能導致被代碼生成器覆蓋
 *如果要增加方法請在當前目錄下Partial文件夾Demo_CustomerController編寫
 */
using Microsoft.AspNetCore.Mvc;
using VOL.Core.Controllers.Basic;
using VOL.Entity.AttributeManager;
using VOL.Demo.IServices;
namespace VOL.Demo.Controllers
{
    [Route("api/Demo_Customer")]
    [PermissionTable(Name = "Demo_Customer")]
    public partial class Demo_CustomerController : ApiBaseController<IDemo_CustomerService>
    {
        public Demo_CustomerController(IDemo_CustomerService service)
        : base(service)
        {
        }
    }
}

