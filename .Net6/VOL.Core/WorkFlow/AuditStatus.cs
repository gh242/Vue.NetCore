using System;
using System.Collections.Generic;
using System.Text;

namespace VOL.Core.WorkFlow
{
    public enum AuditStatus
    {
        待審核 = 0,
        審核通過 = 1,
        審核中 = 2,
        審核未通過 = 3,
        駁回 = 4
    }

    public enum AuditType
    {
        用戶審批 = 1,
        角色審批 = 2,
        部門審批 = 3
    }
}
