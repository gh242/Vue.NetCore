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
    [Entity(TableCnName = "審批節點",TableName = "Sys_WorkFlowTableStep")]
    public partial class Sys_WorkFlowTableStep:BaseEntity
    {
        /// <summary>
       ///
       /// </summary>
       [Key]
       [Display(Name ="Sys_WorkFlowTableStep_Id")]
       [Column(TypeName="uniqueidentifier")]
       [Required(AllowEmptyStrings=false)]
       public Guid Sys_WorkFlowTableStep_Id { get; set; }

       /// <summary>
       ///主表id
       /// </summary>
       [Display(Name ="主表id")]
       [Column(TypeName="uniqueidentifier")]
       [Required(AllowEmptyStrings=false)]
       public Guid WorkFlowTable_Id { get; set; }

       /// <summary>
       ///流程id
       /// </summary>
       [Display(Name ="流程id")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? WorkFlow_Id { get; set; }

       /// <summary>
       ///節點id
       /// </summary>
       [Display(Name ="節點id")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       public string StepId { get; set; }

       /// <summary>
       ///節名稱
       /// </summary>
       [Display(Name ="節名稱")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       public string StepName { get; set; }

       /// <summary>
       ///審批類型
       /// </summary>
       [Display(Name ="審批類型")]
       [Column(TypeName="int")]
       public int? StepType { get; set; }

       /// <summary>
       ///節點類型(1=按用戶審批,2=按角色審批)
       /// </summary>
       [Display(Name ="節點類型(1=按用戶審批,2=按角色審批)")]
       [MaxLength(500)]
       [Column(TypeName="varchar(500)")]
       public string StepValue { get; set; }

       /// <summary>
       ///審批順序
       /// </summary>
       [Display(Name ="審批順序")]
       [Column(TypeName="int")]
       public int? OrderId { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Remark")]
       [MaxLength(500)]
       [Column(TypeName="nvarchar(500)")]
       public string Remark { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="CreateDate")]
       [Column(TypeName="datetime")]
       public DateTime? CreateDate { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="CreateID")]
       [Column(TypeName="int")]
       public int? CreateID { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Creator")]
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
       ///
       /// </summary>
       [Display(Name ="Modifier")]
       [MaxLength(30)]
       [Column(TypeName="nvarchar(30)")]
       public string Modifier { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="ModifyDate")]
       [Column(TypeName="datetime")]
       public DateTime? ModifyDate { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="ModifyID")]
       [Column(TypeName="int")]
       public int? ModifyID { get; set; }

       /// <summary>
       ///審核人id
       /// </summary>
       [Display(Name ="審核人id")]
       [Column(TypeName="int")]
       public int? AuditId { get; set; }

       /// <summary>
       ///審核人
       /// </summary>
       [Display(Name ="審核人")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       public string Auditor { get; set; }

       /// <summary>
       ///審核狀態
       /// </summary>
       [Display(Name ="審核狀態")]
       [Column(TypeName="int")]
       public int? AuditStatus { get; set; }

       /// <summary>
       ///審核時間
       /// </summary>
       [Display(Name ="審核時間")]
       [Column(TypeName="datetime")]
       public DateTime? AuditDate { get; set; }

       /// <summary>
       ///節點屬性(start、node、end))
       /// </summary>
       [Display(Name ="節點屬性(start、node、end))")]
       [MaxLength(50)]
       [Column(TypeName="varchar(50)")]
       public string StepAttrType { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="ParentId")]
       [MaxLength(2000)]
       [Column(TypeName="varchar(2000)")]
       public string ParentId { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="NextStepId")]
       [MaxLength(100)]
       [Column(TypeName="varchar(100)")]
       public string NextStepId { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Weight")]
       [Column(TypeName="int")]
       public int? Weight { get; set; }

       
    }
}