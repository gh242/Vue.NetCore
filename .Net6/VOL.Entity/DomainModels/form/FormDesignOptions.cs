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
    [Entity(TableCnName = "表單設計",TableName = "FormDesignOptions")]
    public class FormDesignOptions:BaseEntity
    {
        /// <summary>
       ///
       /// </summary>
       [Key]
       [Display(Name ="FormId")]
       [Column(TypeName="uniqueidentifier")]
       [Required(AllowEmptyStrings=false)]
       public Guid FormId { get; set; }

       /// <summary>
       ///表單名稱
       /// </summary>
       [Display(Name ="表單名稱")]
       [MaxLength(1000)]
       [Column(TypeName="nvarchar(1000)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string Title { get; set; }

       /// <summary>
       ///設計器配置
       /// </summary>
       [Display(Name ="設計器配置")]
       [Column(TypeName="nvarchar(max)")]
       [Editable(true)]
       public string DaraggeOptions { get; set; }

       /// <summary>
       ///表單參數
       /// </summary>
       [Display(Name ="表單參數")]
       [Column(TypeName="nvarchar(max)")]
       [Editable(true)]
       public string FormOptions { get; set; }

       /// <summary>
       ///表單配置
       /// </summary>
       [Display(Name ="表單配置")]
       [Column(TypeName="nvarchar(max)")]
       [Editable(true)]
       public string FormConfig { get; set; }

       /// <summary>
       ///表單字段
       /// </summary>
       [Display(Name ="表單字段")]
       [Column(TypeName="nvarchar(max)")]
       [Editable(true)]
       public string FormFields { get; set; }

       /// <summary>
       ///表格配置
       /// </summary>
       [Display(Name ="表格配置")]
       [Column(TypeName="nvarchar(max)")]
       [Editable(true)]
       public string TableConfig { get; set; }

       /// <summary>
       ///創建人
       /// </summary>
       [Display(Name ="創建人")]
       [MaxLength(60)]
       [Column(TypeName="nvarchar(60)")]
       public string Creator { get; set; }

       /// <summary>
       ///創建時間
       /// </summary>
       [Display(Name ="創建時間")]
       [Column(TypeName="datetime")]
       public DateTime? CreateDate { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="CreateID")]
       [Column(TypeName="int")]
       public int? CreateID { get; set; }

       /// <summary>
       ///修改人
       /// </summary>
       [Display(Name ="修改人")]
       [MaxLength(60)]
       [Column(TypeName="nvarchar(60)")]
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

       
    }
}