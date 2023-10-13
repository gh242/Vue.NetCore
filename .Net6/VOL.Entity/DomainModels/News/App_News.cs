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
    [Table("App_News")]
    [Entity(TableCnName = "新聞列表",TableName = "App_News")]
    public class App_News:BaseEntity
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
       ///標題
       /// </summary>
       [Display(Name ="標題")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string Title { get; set; }

       /// <summary>
       ///新聞内容
       /// </summary>
       [Display(Name ="新聞内容")]
       [Column(TypeName="nvarchar(max)")]
       [Editable(true)]
       public string Content { get; set; }

       /// <summary>
       ///發佈人
       /// </summary>
       [Display(Name ="發佈人")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       public string Author { get; set; }

       /// <summary>
       ///發佈時間
       /// </summary>
       [Display(Name ="發佈時間")]
       [Column(TypeName="datetime")]
       public DateTime? ReleaseDate { get; set; }

       /// <summary>
       ///封面圖片
       /// </summary>
       [Display(Name ="封面圖片")]
       [MaxLength(500)]
       [Column(TypeName="nvarchar(500)")]
       [Editable(true)]
       public string ImageUrl { get; set; }

       /// <summary>
       ///圖片(大圖)
       /// </summary>
       [Display(Name ="圖片(大圖)")]
       [MaxLength(500)]
       [Column(TypeName="nvarchar(500)")]
       public string BigImageUrls { get; set; }

       /// <summary>
       ///新聞地址
       /// </summary>
       [Display(Name ="新聞地址")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       [Editable(true)]
       public string DetailUrl { get; set; }

       /// <summary>
       ///瀏覽次數
       /// </summary>
       [Display(Name ="瀏覽次數")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? ViewCount { get; set; }

       /// <summary>
       ///新聞類型
       /// </summary>
       [Display(Name ="新聞類型")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int NewsType { get; set; }

       /// <summary>
       ///是否今日推薦
       /// </summary>
       [Display(Name ="是否今日推薦")]
       [Column(TypeName="sbyte")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public sbyte DailyRecommend { get; set; }

       /// <summary>
       ///推薦排序
       /// </summary>
       [Display(Name ="推薦排序")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? OrderNo { get; set; }

       /// <summary>
       ///是否啟用
       /// </summary>
       [Display(Name ="是否啟用")]
       [Column(TypeName="sbyte")]
       [Editable(true)]
       public sbyte? Enable { get; set; }

       /// <summary>
       ///創建人Id
       /// </summary>
       [Display(Name ="創建人Id")]
       [Column(TypeName="int")]
       public int? CreateID { get; set; }

       /// <summary>
       ///發佈人
       /// </summary>
       [Display(Name ="發佈人")]
       [MaxLength(30)]
       [Column(TypeName="nvarchar(30)")]
       [Editable(true)]
       public string Creator { get; set; }

       /// <summary>
       ///發佈時間
       /// </summary>
       [Display(Name ="發佈時間")]
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
       [Editable(true)]
       public DateTime? ModifyDate { get; set; }

       
    }
}
