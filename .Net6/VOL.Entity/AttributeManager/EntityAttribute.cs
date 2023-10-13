using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOL.Entity
{
    public class EntityAttribute : Attribute
    {
        /// <summary>
        /// 真實表名(數據庫表名，若沒有填寫默認實體為表名)
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 表中文名
        /// </summary>
        public string TableCnName { get; set; }
        /// <summary>
        /// 子表
        /// </summary>
        public Type[] DetailTable { get; set; }
        /// <summary>
        /// 子表中文名
        /// </summary>
        public string DetailTableCnName { get; set; }
        /// <summary>
        /// 數據庫
        /// </summary>
        public string DBServer { get; set; }

        //是否開啟用戶數據權限,true=用戶只能操作自己(及下級角色)創建的數據,如:查詢、刪除、修改等操作
        public bool CurrentUserPermission { get; set; }

        public Type ApiInput { get; set; }
        public Type ApiOutput { get; set; }
    }
}
