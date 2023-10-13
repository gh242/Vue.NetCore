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
    [Entity(TableCnName = "執行記錄",TableName = "Sys_QuartzLog")]
    public partial class Sys_QuartzLog:BaseEntity
    {
        /// <summary>
       ///
       /// </summary>
       [Key]
       [Display(Name ="LogId")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public Guid LogId { get; set; }

       /// <summary>
       ///任務id
       /// </summary>
       [Display(Name ="任務id")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? Id { get; set; }

       /// <summary>
       ///任務名稱
       /// </summary>
       [Display(Name ="任務名稱")]
       [MaxLength(500)]
       [Column(TypeName="nvarchar(500)")]
       [Editable(true)]
       public string TaskName { get; set; }

       /// <summary>
       ///耗時(秒)
       /// </summary>
       [Display(Name ="耗時(秒)")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? ElapsedTime { get; set; }

       /// <summary>
       ///開始時間
       /// </summary>
       [Display(Name ="開始時間")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? StratDate { get; set; }

       /// <summary>
       ///結束時間
       /// </summary>
       [Display(Name ="結束時間")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? EndDate { get; set; }

       /// <summary>
       ///是否成功
       /// </summary>
       [Display(Name ="是否成功")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? Result { get; set; }

       /// <summary>
       ///返回内容
       /// </summary>
       [Display(Name ="返回内容")]
       [Column(TypeName="nvarchar(max)")]
       [Editable(true)]
       public string ResponseContent { get; set; }

       /// <summary>
       ///異常信息
       /// </summary>
       [Display(Name ="異常信息")]
       [Column(TypeName="nvarchar(max)")]
       public string ErrorMsg { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="CreateID")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? CreateID { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Creator")]
       [MaxLength(30)]
       [Column(TypeName="nvarchar(30)")]
       [Editable(true)]
       public string Creator { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="CreateDate")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? CreateDate { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="ModifyID")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? ModifyID { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Modifier")]
       [MaxLength(30)]
       [Column(TypeName="nvarchar(30)")]
       [Editable(true)]
       public string Modifier { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="ModifyDate")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? ModifyDate { get; set; }

       
    }
}