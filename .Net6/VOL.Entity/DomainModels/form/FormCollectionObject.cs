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
    [Entity(TableCnName = "數據採集",TableName = "FormCollectionObject")]
    public class FormCollectionObject:BaseEntity
    {
        /// <summary>
       ///
       /// </summary>
       [Key]
       [Display(Name ="FormCollectionId")]
       [Column(TypeName="uniqueidentifier")]
       [Required(AllowEmptyStrings=false)]
       public Guid FormCollectionId { get; set; }

       /// <summary>
       ///表單ID
       /// </summary>
       [Display(Name ="表單ID")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? FormId { get; set; }

       /// <summary>
       ///標題
       /// </summary>
       [Display(Name ="標題")]
       [Column(TypeName="nvarchar(max)")]
       [Editable(true)]
       public string Title { get; set; }

       /// <summary>
       ///表單數據
       /// </summary>
       [Display(Name ="表單數據")]
       [Column(TypeName="nvarchar(max)")]
       [Editable(true)]
       public string FormData { get; set; }

       /// <summary>
       ///提交人
       /// </summary>
       [Display(Name ="提交人")]
       [MaxLength(60)]
       [Column(TypeName="nvarchar(60)")]
       public string Creator { get; set; }

       /// <summary>
       ///提交時間
       /// </summary>
       [Display(Name ="提交時間")]
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
       [Display(Name ="Modifier")]
       [MaxLength(60)]
       [Column(TypeName="nvarchar(60)")]
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

       
    }
}