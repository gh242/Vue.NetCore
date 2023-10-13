using Newtonsoft.Json;
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
    [Entity(TableCnName = "訂單詳情",TableName = "View_OrderInfo")]
    public partial class View_OrderInfo:BaseEntity
    {
        /// <summary>
       ///
       /// </summary>
       [Key]
       [Display(Name ="Order_Id")]
       [JsonIgnore]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public Guid Order_Id { get; set; }

       /// <summary>
       ///訂單編號
       /// </summary>
       [Display(Name ="訂單編號")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
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
       ///訂單數量
       /// </summary>
       [Display(Name ="訂單數量")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? TotalQty { get; set; }

       /// <summary>
       ///訂單金額
       /// </summary>
       [Display(Name ="訂單金額")]
       [DisplayFormat(DataFormatString="18,2")]
       [Column(TypeName="decimal")]
       [Editable(true)]
       public decimal? TotalPrice { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="GoodsId")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? GoodsId { get; set; }

       /// <summary>
       ///商品編號
       /// </summary>
       [Display(Name ="商品編號")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string GoodsCode { get; set; }

       /// <summary>
       ///商品名稱
       /// </summary>
       [Display(Name ="商品名稱")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string GoodsName { get; set; }

       /// <summary>
       ///商品規格
       /// </summary>
       [Display(Name ="商品規格")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string Specs { get; set; }

       /// <summary>
       ///單價
       /// </summary>
       [Display(Name ="單價")]
       [DisplayFormat(DataFormatString="18,2")]
       [Column(TypeName="decimal")]
       [Editable(true)]
       public decimal? Price { get; set; }

       /// <summary>
       ///數量
       /// </summary>
       [Display(Name ="數量")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? Qty { get; set; }

       /// <summary>
       ///商品圖片
       /// </summary>
       [Display(Name ="商品圖片")]
       [MaxLength(500)]
       [Column(TypeName="nvarchar(500)")]
       [Editable(true)]
       public string Img { get; set; }

       /// <summary>
       ///訂單日期
       /// </summary>
       [Display(Name ="訂單日期")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public DateTime OrderDate { get; set; }

       /// <summary>
       ///創建人
       /// </summary>
       [Display(Name ="創建人")]
       [MaxLength(30)]
       [Column(TypeName="nvarchar(30)")]
       [Editable(true)]
       public string Creator { get; set; }

       
    }
}
