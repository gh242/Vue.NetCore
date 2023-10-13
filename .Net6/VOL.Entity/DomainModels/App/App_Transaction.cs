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
    [Entity(TableCnName = "導入導出",TableName = "App_Transaction")]
    public class App_Transaction:BaseEntity
    {
        /// <summary>
       ///主鍵ID
       /// </summary>
       [Key]
       [Display(Name ="主鍵ID")]
       [Column(TypeName="int")]
       [Required(AllowEmptyStrings=false)]
       public int Id { get; set; }

       /// <summary>
       ///姓名
       /// </summary>
       [Display(Name ="姓名")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string Name { get; set; }

       /// <summary>
       ///電話
       /// </summary>
       [Display(Name ="電話")]
       [MaxLength(15)]
       [Column(TypeName="nvarchar(15)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string PhoneNo { get; set; }

       /// <summary>
       ///數量
       /// </summary>
       [Display(Name ="數量")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int Quantity { get; set; }

       /// <summary>
       ///是否買入
       /// </summary>
       [Display(Name ="是否買入")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int TransactionType { get; set; }

       /// <summary>
       ///购買類型
       /// </summary>
       [Display(Name ="购買類型")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string CowType { get; set; }

       /// <summary>
       ///描述
       /// </summary>
       [Display(Name ="描述")]
       [MaxLength(500)]
       [Column(TypeName="nvarchar(500)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string Describe { get; set; }

       /// <summary>
       ///是否啟用
       /// </summary>
       [Display(Name ="是否啟用")]
       [Column(TypeName="sbyte")]
       public sbyte? Enable { get; set; }

       /// <summary>
       ///創建人Id
       /// </summary>
       [Display(Name ="創建人Id")]
       [Column(TypeName="int")]
       public int? CreateID { get; set; }

       /// <summary>
       ///提交人
       /// </summary>
       [Display(Name ="提交人")]
       [MaxLength(30)]
       [Column(TypeName="nvarchar(30)")]
       [Editable(true)]
       public string Creator { get; set; }

       /// <summary>
       ///提交時間
       /// </summary>
       [Display(Name ="提交時間")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? CreateDate { get; set; }

       /// <summary>
       ///修改人ID
       /// </summary>
       [Display(Name ="修改人ID")]
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

       
    }
}