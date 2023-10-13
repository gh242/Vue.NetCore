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
    [Entity(TableCnName = "test2019")]
    public class test2019:BaseEntity
    {
        /// <summary>
       ///測試Id
       /// </summary>
       [Key]
       [Display(Name ="測試Id")]
       [Column(TypeName="int")]
       [Required(AllowEmptyStrings=false)]
       public int id { get; set; }

       /// <summary>
       ///測試1
       /// </summary>
       [Display(Name ="測試1")]
       [MaxLength(10)]
       [Column(TypeName="nvarchar(10)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string testUtf8 { get; set; }

       /// <summary>
       ///測試2
       /// </summary>
       [Display(Name ="測試2")]
       [MaxLength(12)]
       [Column(TypeName="nvarchar(12)")]
       [Editable(true)]
       public string testmb4 { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="y")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? y { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="timepsan")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? timepsan { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="text")]
       [MaxLength(65535)]
       [Column(TypeName="nvarchar(65535)")]
       [Editable(true)]
       public string text { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="db")]
       [DisplayFormat(DataFormatString="4,3")]
       [Column(TypeName="decimal")]
       [Editable(true)]
       public decimal? db { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="tt")]
       [MaxLength(1)]
       [Column(TypeName="nvarchar(1)")]
       [Editable(true)]
       public string tt { get; set; }

       
    }
}