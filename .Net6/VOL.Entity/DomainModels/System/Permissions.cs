using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOL.Entity.DomainModels
{
    public class Permissions
    {
        public int Menu_Id { get; set; }
        public int ParentId { get; set; }
        public string TableName { get; set; }
        public string MenuAuth { get; set; }
        public string UserAuth { get; set; }
        /// <summary>
        /// 當前用戶權限,存储的是權限的值，如:Add,Search等
        /// </summary>
        public string[] UserAuthArr { get; set; }

        /// <summary>
        /// 2022.03.26
        /// 菜單類型1:移動端，0:PC端
        /// </summary>
        public int MenuType { get; set; }
    }
}
