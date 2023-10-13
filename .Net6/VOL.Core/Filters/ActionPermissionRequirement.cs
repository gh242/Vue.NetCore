using System;
using System.Collections.Generic;
using System.Text;

namespace VOL.Core.Filters
{
    public class ActionPermissionRequirement
    {
        /// <summary>
        /// 控制器要操作的表
        /// </summary>
        public string TableName { get; set; } = string.Empty;
        /// <summary>
        /// 對表的操作/刪除/查詢等
        /// </summary>
        public string TableAction { get; set; } = string.Empty;
        /// <summary>
        /// 是否為框架定義的控制器
        /// </summary>
        public bool SysController { get; set; }
        /// <summary>
        /// 限制只能由某些角色Id訪問
        /// </summary>
        public int[] RoleIds { get; set; }

        public bool IsApi { get; set; }
    }
}
