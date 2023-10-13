using Newtonsoft.Json;
/*
 *Author：jxx
 *Contact：283591387@qq.com
 *Date：2018-07-01
 * 此代碼由框架生成，請勿随意更改
 */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VOL.Entity.SystemModels;

namespace VOL.Entity.DomainModels
{
    [Table("Sys_Role")]
    [EntityAttribute(TableCnName = "角色管理")]
    public class Sys_Role : BaseEntity
    {
        /// <summary>
        ///Id
        /// </summary>
        [Key]
        [Display(Name = "Id")]
        [Column(TypeName = "int")]
        [Required(AllowEmptyStrings = false)]
        public int Role_Id { get; set; }

        /// <summary>
        ///父級ID
        /// </summary>
        [Display(Name = "父級ID")]
        [Column(TypeName = "int")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public int ParentId { get; set; }

        /// <summary>
        ///角色名稱
        /// </summary>
        [Display(Name = "角色名稱")]
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        [Editable(true)]
        public string RoleName { get; set; }

        /// <summary>
        ///部門ID
        /// </summary>
        [Display(Name = "部門ID")]
        [Column(TypeName = "int")]
        [Editable(true)]
        public int? Dept_Id { get; set; }

        /// <summary>
        ///部門名稱
        /// </summary>
        [Display(Name = "部門名稱")]
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        [Editable(true)]
        public string DeptName { get; set; }

        /// <summary>
        ///排序
        /// </summary>
        [Display(Name = "排序")]
        [Column(TypeName = "int")]
        [Editable(true)]
        public int? OrderNo { get; set; }

        /// <summary>
        ///創建人
        /// </summary>
        [Display(Name = "創建人")]
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        [Editable(true)]
        public string Creator { get; set; }

        /// <summary>
        ///創建時間
        /// </summary>
        [Display(Name = "創建時間")]
        [Column(TypeName = "datetime")]
        [Editable(true)]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        ///修改人
        /// </summary>
        [Display(Name = "修改人")]
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        [Editable(true)]
        public string Modifier { get; set; }

        /// <summary>
        ///修改時間
        /// </summary>
        [Display(Name = "修改時間")]
        [Column(TypeName = "datetime")]
        [Editable(true)]
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Display(Name = "DeleteBy")]
        [MaxLength(50)]
        [JsonIgnore]
        [Column(TypeName = "nvarchar(50)")]
        public string DeleteBy { get; set; }

        /// <summary>
        ///是否啟用
        /// </summary>
        [Display(Name = "是否啟用")]
        [Column(TypeName = "tinyint")]
        [Editable(true)]
        public byte? Enable { get; set; }
        [ForeignKey("Role_Id")]
        public List<Sys_RoleAuth> RoleAuths { get; set; }

    }
}

