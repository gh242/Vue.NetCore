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
    [Table("Sys_Dictionary")]
    [Entity(TableCnName = "字典數據",DetailTable =  new Type[] { typeof(Sys_DictionaryList)},DetailTableCnName = "字典明細")]
    public class Sys_Dictionary:BaseEntity
    {
        /// <summary>
       ///字典ID
       /// </summary>
       [Key]
       [Display(Name ="字典ID")]
       [Column(TypeName="int")]
       [Required(AllowEmptyStrings=false)]
       public int Dic_ID { get; set; }

       /// <summary>
       ///字典編號
       /// </summary>
       [Display(Name ="字典編號")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string DicNo { get; set; }

       /// <summary>
       ///字典名稱
       /// </summary>
       [Display(Name ="字典名稱")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string DicName { get; set; }

       /// <summary>
       ///父級ID
       /// </summary>
       [Display(Name ="父級ID")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int ParentId { get; set; }

       /// <summary>
       ///配置項
       /// </summary>
       [Display(Name ="配置項")]
       [MaxLength(10000)]
       [Column(TypeName="nvarchar(10000)")]
       [Editable(true)]
       public string Config { get; set; }

       /// <summary>
       ///sql語句
       /// </summary>
       [Display(Name ="sql語句")]
       [MaxLength(10000)]
       [Column(TypeName="nvarchar(10000)")]
       [Editable(true)]
       public string DbSql { get; set; }

       /// <summary>
       ///DBServer
       /// </summary>
       [Display(Name ="DBServer")]
       [MaxLength(10000)]
       [Column(TypeName="nvarchar(10000)")]
       public string DBServer { get; set; }

       /// <summary>
       ///排序號
       /// </summary>
       [Display(Name ="排序號")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? OrderNo { get; set; }

       /// <summary>
       ///備註
       /// </summary>
       [Display(Name ="備註")]
       [MaxLength(2000)]
       [Column(TypeName="nvarchar(2000)")]
       [Editable(true)]
       public string Remark { get; set; }

       /// <summary>
       ///是否啟用
       /// </summary>
       [Display(Name ="是否啟用")]
       [Column(TypeName="tinyint")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public byte Enable { get; set; }

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
       ///創建時間
       /// </summary>
       [Display(Name ="創建時間")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? CreateDate { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="ModifyID")]
       [Column(TypeName="int")]
       public int? ModifyID { get; set; }

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

       [Display(Name ="字典明細")]
       [ForeignKey("Dic_ID")]
       public List<Sys_DictionaryList> Sys_DictionaryList { get; set; }

    }
}