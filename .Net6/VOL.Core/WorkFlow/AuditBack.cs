using System;
using System.Collections.Generic;
using System.Text;

namespace VOL.Core.WorkFlow
{
    public enum AuditBack
    {
        流程結束 = 0,
        返回上一節點 = 1,
        流程重新開始 = 2
    }
}
