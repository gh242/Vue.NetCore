using System;

namespace VOL.Entity.AttributeManager
{
    public class PermissionTableAttribute : Attribute
    {
        /// <summary>
        /// 需要控制權限的表名與Sys_Menu表的表名必須一致
        /// </summary>
        public string Name { get; set; }
    }
}
