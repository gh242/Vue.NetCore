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
    [Entity(TableCnName = "啟用圖片支持")]
    public class App_Expert:BaseEntity
    {
        /// <summary>
       ///主鍵ID
       /// </summary>
       [Key]
       [Display(Name ="主鍵ID")]
       [Column(TypeName="int")]
       [Required(AllowEmptyStrings=false)]
       public int ExpertId { get; set; }

       /// <summary>
       ///申請人帳號Id
       /// </summary>
       [Display(Name ="申請人帳號Id")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? User_Id { get; set; }

       /// <summary>
       ///專家名稱
       /// </summary>
       [Display(Name ="專家名稱")]
       [MaxLength(20)]
       [Column(TypeName="nvarchar(20)")]
       [Editable(true)]
       public string ExpertName { get; set; }

       /// <summary>
       ///專家頭像
       /// </summary>
       [Display(Name ="專家頭像")]
       [MaxLength(500)]
       [Column(TypeName="nvarchar(500)")]
       [Editable(true)]
       public string HeadImageUrl { get; set; }

       /// <summary>
       ///申請人帳號
       /// </summary>
       [Display(Name ="申請人帳號")]
       [MaxLength(30)]
       [Column(TypeName="nvarchar(30)")]
       [Editable(true)]
       public string UserName { get; set; }

       /// <summary>
       ///申請人
       /// </summary>
       [Display(Name ="申請人")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       public string UserTrueName { get; set; }

       /// <summary>
       ///審核狀態
       /// </summary>
       [Display(Name ="審核狀態")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int AuditStatus { get; set; }

       /// <summary>
       ///審核人
       /// </summary>
       [Display(Name ="審核人")]
       [MaxLength(20)]
       [Column(TypeName="nvarchar(20)")]
       [Editable(true)]
       public string Auditor { get; set; }

       /// <summary>
       ///是否啟用
       /// </summary>
       [Display(Name ="是否啟用")]
       [Column(TypeName="tinyint")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public byte Enable { get; set; }

       /// <summary>
       ///真實姓名
       /// </summary>
       [Display(Name ="真實姓名")]
       [MaxLength(20)]
       [Column(TypeName="nvarchar(20)")]
       [Editable(true)]
       public string ReallyName { get; set; }

       /// <summary>
       ///身份證號
       /// </summary>
       [Display(Name ="身份證號")]
       [MaxLength(18)]
       [Column(TypeName="nvarchar(18)")]
       [Editable(true)]
       public string IDNumber { get; set; }

       /// <summary>
       ///電話
       /// </summary>
       [Display(Name ="電話")]
       [MaxLength(11)]
       [Column(TypeName="nvarchar(11)")]
       [Editable(true)]
       public string PhoneNo { get; set; }

       /// <summary>
       ///學歷
       /// </summary>
       [Display(Name ="學歷")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       public string Education { get; set; }

       /// <summary>
       ///職業
       /// </summary>
       [Display(Name ="職業")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       public string Professional { get; set; }

       /// <summary>
       ///所在公司
       /// </summary>
       [Display(Name ="所在公司")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       public string Company { get; set; }

       /// <summary>
       ///服務地區
       /// </summary>
       [Display(Name ="服務地區")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       public string City { get; set; }

       /// <summary>
       ///擅長領域
       /// </summary>
       [Display(Name ="擅長領域")]
       [MaxLength(800)]
       [Column(TypeName="nvarchar(800)")]
       [Editable(true)]
       public string SpecialField { get; set; }

       /// <summary>
       ///個人簡介
       /// </summary>
       [Display(Name ="個人簡介")]
       [MaxLength(500)]
       [Column(TypeName="nvarchar(500)")]
       [Editable(true)]
       public string Resume { get; set; }

       /// <summary>
       ///資質證書
       /// </summary>
       [Display(Name ="資質證書")]
       [MaxLength(2500)]
       [Column(TypeName="nvarchar(2500)")]
       [Editable(true)]
       public string  Certificate { get; set; }

       /// <summary>
       ///審核人Id
       /// </summary>
       [Display(Name ="審核人Id")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? AuditId { get; set; }

       /// <summary>
       ///審核時間
       /// </summary>
       [Display(Name ="審核時間")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? AuditDate { get; set; }

       /// <summary>
       ///創建人Id
       /// </summary>
       [Display(Name ="創建人Id")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? CreateID { get; set; }

       /// <summary>
       ///創建人
       /// </summary>
       [Display(Name ="創建人")]
       [MaxLength(30)]
       [Column(TypeName="nvarchar(30)")]
       [Editable(true)]
       public string Creator { get; set; }

       /// <summary>
       ///申請時間
       /// </summary>
       [Display(Name ="申請時間")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? CreateDate { get; set; }

       /// <summary>
       ///修改人ID
       /// </summary>
       [Display(Name ="修改人ID")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? ModifyID { get; set; }

       /// <summary>
       ///修改人
       /// </summary>
       [Display(Name ="修改人")]
       [MaxLength(30)]
       [Column(TypeName="nvarchar(30)")]
       [Editable(true)]
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