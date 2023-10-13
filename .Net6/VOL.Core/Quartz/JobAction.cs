using System;
using System.Collections.Generic;
using System.Text;

namespace VOL.Core.Quartz
{
    public enum JobAction
    {
        新增 = 1,
        刪除 = 2,
        修改 = 3,
        暫停 = 4,
        停止,
        開啟,
        立即執行
    }
}
