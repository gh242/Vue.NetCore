/*
 *代碼由框架生成,任何更改都可能導致被代碼生成器覆蓋
 *如果要增加方法請在當前目錄下Partial文件夾test2019Controller編寫
 */
using Microsoft.AspNetCore.Mvc;
using VOL.AppManager.IServices;
using VOL.Core.Controllers.Basic;
using VOL.Entity.AttributeManager;

namespace VOL.AppManager.Controllers
{
    [Route("api/test2019")]
    [PermissionTable(Name = "test2019")]
    public partial class test2019Controller : ApiBaseController<Itest2019Service>
    {
        public test2019Controller(Itest2019Service service)
        : base("AppManager","App","test2019", service)
        {
        }
    }
}

