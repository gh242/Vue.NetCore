/*
 *Author：jxx
 *Contact：283591387@qq.com
 *Date：2018-07-01
 * 此代碼由框架生成，請勿随意更改
 */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOL.Entity;
using VOL.Entity.SystemModels;

namespace VOL.Entity.DomainModels
{
    [Table("Sys_Log")]
    [EntityAttribute(TableCnName = "系統日誌")]
    public class Sys_Log:BaseEntity
    {
        /// <summary>
       ///
       /// </summary>
       [Key]
       [Display(Name ="Id")]
       [Column(TypeName="int")]
       [Required(AllowEmptyStrings=false)]
       public int Id { get; set; }

       /// <summary>
       ///開始時間
       /// </summary>
       [Display(Name ="開始時間")]
       [Column(TypeName="datetime")]
       public DateTime? BeginDate { get; set; }

       /// <summary>
       ///請求地址
       /// </summary>
       [Display(Name ="請求地址")]
       [MaxLength(30000)]
       [Column(TypeName="varchar(30000)")]
       public string Url { get; set; }

       /// <summary>
       ///日誌類型
       /// </summary>
       [Display(Name ="日誌類型")]
       [MaxLength(50)]
       [Column(TypeName="varchar(50)")]
       public string LogType { get; set; }

       /// <summary>
       ///響應狀態
       /// </summary>
       [Display(Name ="響應狀態")]
       [Column(TypeName="int")]
       public int? Success { get; set; }

       /// <summary>
       ///時長(毫秒)
       /// </summary>
       [Display(Name ="時長(毫秒)")]
       [Column(TypeName="int")]
       public int? ElapsedTime { get; set; }

       /// <summary>
       ///請求參數
       /// </summary>
       [Display(Name ="請求參數")]
       [MaxLength(10000)]
       [Column(TypeName="nvarchar(10000)")]
       public string RequestParameter { get; set; }

       /// <summary>
       ///響應參數
       /// </summary>
       [Display(Name ="響應參數")]
       [MaxLength(10000)]
       [Column(TypeName="nvarchar(10000)")]
       public string ResponseParameter { get; set; }

       /// <summary>
       ///異常信息
       /// </summary>
       [Display(Name ="異常信息")]
       [MaxLength(10000)]
       [Column(TypeName="nvarchar(10000)")]
       public string ExceptionInfo { get; set; }

       /// <summary>
       ///用戶IP
       /// </summary>
       [Display(Name ="用戶IP")]
       [MaxLength(100)]
       [Column(TypeName="varchar(100)")]
       public string UserIP { get; set; }

       /// <summary>
       ///服務器IP
       /// </summary>
       [Display(Name ="服務器IP")]
       [MaxLength(100)]
       [Column(TypeName="varchar(100)")]
       public string ServiceIP { get; set; }

       /// <summary>
       ///瀏覽器類型
       /// </summary>
       [Display(Name ="瀏覽器類型")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       public string BrowserType { get; set; }

       /// <summary>
       ///用戶ID
       /// </summary>
       [Display(Name ="用戶ID")]
       [Column(TypeName="int")]
       public int? User_Id { get; set; }

       /// <summary>
       ///用戶名稱
       /// </summary>
       [Display(Name ="用戶名稱")]
       [MaxLength(30000)]
       [Column(TypeName="varchar(30000)")]
       public string UserName { get; set; }

       /// <summary>
       ///角色ID
       /// </summary>
       [Display(Name ="角色ID")]
       [Column(TypeName="int")]
       public int? Role_Id { get; set; }

       /// <summary>
       ///結束時間
       /// </summary>
       [Display(Name ="結束時間")]
       [Column(TypeName="datetime")]
       public DateTime? EndDate { get; set; }

       
    }
}
