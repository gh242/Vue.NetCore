/*
 *代碼由框架生成,任何更改都可能導致被代碼生成器覆蓋
 *如果數據庫字段發生變化，請在代碼生器重新生成此Model
 */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOL.Entity.SystemModels;

namespace VOL.Entity.DomainModels
{
    [Entity(TableCnName = "審批節點配置",TableName = "Sys_WorkFlowStep")]
    public partial class Sys_WorkFlowStep:BaseEntity
    {
        /// <summary>
       ///
       /// </summary>
       [Key]
       [Display(Name ="WorkStepFlow_Id")]
       [Column(TypeName="uniqueidentifier")]
       [Required(AllowEmptyStrings=false)]
       public Guid WorkStepFlow_Id { get; set; }

       /// <summary>
       ///流程主表id
       /// </summary>
       [Display(Name ="流程主表id")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? WorkFlow_Id { get; set; }

       /// <summary>
       ///流程節點Id
       /// </summary>
       [Display(Name ="流程節點Id")]
       [MaxLength(100)]
       [Column(TypeName="varchar(100)")]
       [Editable(true)]
       public string StepId { get; set; }

       /// <summary>
       ///節點名稱
       /// </summary>
       [Display(Name ="節點名稱")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       [Editable(true)]
       public string StepName { get; set; }

       /// <summary>
       ///節點類型(1=按用戶審批,2=按角色審批)
       /// </summary>
       [Display(Name ="節點類型(1=按用戶審批,2=按角色審批)")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? StepType { get; set; }

       /// <summary>
       ///審批用戶id或角色id
       /// </summary>
       [Display(Name ="審批用戶id或角色id")]
       [MaxLength(500)]
       [Column(TypeName="varchar(500)")]
       [Editable(true)]
       public string StepValue { get; set; }

       /// <summary>
       ///備註
       /// </summary>
       [Display(Name ="備註")]
       [MaxLength(500)]
       [Column(TypeName="nvarchar(500)")]
       [Editable(true)]
       public string Remark { get; set; }

       /// <summary>
       ///審批順序
       /// </summary>
       [Display(Name ="審批順序")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? OrderId { get; set; }

       /// <summary>
       ///創建時間
       /// </summary>
       [Display(Name ="創建時間")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? CreateDate { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="CreateID")]
       [Column(TypeName="int")]
       public int? CreateID { get; set; }

       /// <summary>
       ///創建人
       /// </summary>
       [Display(Name ="創建人")]
       [MaxLength(30)]
       [Column(TypeName="nvarchar(30)")]
       public string Creator { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Enable")]
       [Column(TypeName="tinyint")]
       public byte? Enable { get; set; }

       /// <summary>
       ///修改人
       /// </summary>
       [Display(Name ="修改人")]
       [MaxLength(30)]
       [Column(TypeName="nvarchar(30)")]
       public string Modifier { get; set; }

       /// <summary>
       ///修改時間
       /// </summary>
       [Display(Name ="修改時間")]
       [Column(TypeName="datetime")]
       public DateTime? ModifyDate { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="ModifyID")]
       [Column(TypeName="int")]
       public int? ModifyID { get; set; }

       /// <summary>
       ///下一個審批節點
       /// </summary>
       [Display(Name ="下一個審批節點")]
       [MaxLength(500)]
       [Column(TypeName="varchar(500)")]
       [Editable(true)]
       public string NextStepIds { get; set; }

       /// <summary>
       ///父級節點
       /// </summary>
       [Display(Name ="父級節點")]
       [MaxLength(2000)]
       [Column(TypeName="varchar(2000)")]
       [Editable(true)]
       public string ParentId { get; set; }

       /// <summary>
       ///審核未通過(返回上一節點,流程重新開始,流程結束)
       /// </summary>
       [Display(Name ="審核未通過(返回上一節點,流程重新開始,流程結束)")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? AuditRefuse { get; set; }

       /// <summary>
       ///駁回(返回上一節點,流程重新開始,流程結束)
       /// </summary>
       [Display(Name ="駁回(返回上一節點,流程重新開始,流程結束)")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? AuditBack { get; set; }

       /// <summary>
       ///審批方式(啟用會簽)
       /// </summary>
       [Display(Name ="審批方式(啟用會簽)")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? AuditMethod { get; set; }

       /// <summary>
       ///審核後發送郵件通知
       /// </summary>
       [Display(Name ="審核後發送郵件通知")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? SendMail { get; set; }

       /// <summary>
       ///審核條件
       /// </summary>
       [Display(Name ="審核條件")]
       [MaxLength(4000)]
       [Column(TypeName="nvarchar(4000)")]
       [Editable(true)]
       public string Filters { get; set; }

       /// <summary>
       ///節點屬性(start、node、end))
       /// </summary>
       [Display(Name ="節點屬性(start、node、end))")]
       [MaxLength(50)]
       [Column(TypeName="varchar(50)")]
       [Editable(true)]
       public string StepAttrType { get; set; }

       /// <summary>
       ///權重(相同條件權重大的優先匹配)
       /// </summary>
       [Display(Name ="權重(相同條件權重大的優先匹配)")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? Weight { get; set; }

       
    }
}