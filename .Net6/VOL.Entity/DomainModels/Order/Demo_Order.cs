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
    [Entity(TableCnName = "訂單管理2",TableName = "Demo_Order",DetailTable =  new Type[] { typeof(Demo_OrderList)},DetailTableCnName = "訂單明細")]
    public partial class Demo_Order:BaseEntity
    {
        /// <summary>
       ///
       /// </summary>
       [Key]
       [Display(Name ="Order_Id")]
       [Column(TypeName="uniqueidentifier")]
       [Required(AllowEmptyStrings=false)]
       public Guid Order_Id { get; set; }

       /// <summary>
       ///訂單編號
       /// </summary>
       [Display(Name ="訂單編號")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string OrderNo { get; set; }

       /// <summary>
       ///訂單類型
       /// </summary>
       [Display(Name ="訂單類型")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int OrderType { get; set; }

       /// <summary>
       ///總價
       /// </summary>
       [Display(Name ="總價")]
       [DisplayFormat(DataFormatString="18,2")]
       [Column(TypeName="decimal")]
       [Editable(true)]
       public decimal? TotalPrice { get; set; }

       /// <summary>
       ///總數量
       /// </summary>
       [Display(Name ="總數量")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? TotalQty { get; set; }

       /// <summary>
       ///訂單日期
       /// </summary>
       [Display(Name ="訂單日期")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public DateTime OrderDate { get; set; }

       /// <summary>
       ///客戶
       /// </summary>
       [Display(Name ="客戶")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? CustomerId { get; set; }

       /// <summary>
       ///客戶
       /// </summary>
       [Display(Name ="客戶")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       public string Customer { get; set; }

       /// <summary>
       ///手機
       /// </summary>
       [Display(Name ="手機")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       public string PhoneNo { get; set; }

       /// <summary>
       ///訂單狀態
       /// </summary>
       [Display(Name ="訂單狀態")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int OrderStatus { get; set; }

       /// <summary>
       ///備註
       /// </summary>
       [Display(Name ="備註")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       [Editable(true)]
       public string Remark { get; set; }

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
       public DateTime? CreateDate { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="ModifyID")]
       [Column(TypeName="int")]
       public int? ModifyID { get; set; }

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

       [Display(Name ="訂單明細")]
       [ForeignKey("Order_Id")]
       public List<Demo_OrderList> Demo_OrderList { get; set; }

    }
}