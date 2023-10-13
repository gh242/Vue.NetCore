using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOL.Entity;

namespace VOL.Entity.DomainModels
{
    public class ApiSys_UserInput
    {
        /// <summary>
       ///用戶名
       /// </summary>
       [Display(Name ="用戶名")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string UserName { get; set; }

       /// <summary>
       ///密碼
       /// </summary>
       [Display(Name ="密碼")]
       [MaxLength(400)]
       [Column(TypeName="nvarchar(400)")]
       [Required(AllowEmptyStrings=false)]
       public string UserPwd { get; set; }

       /// <summary>
       ///手機號
       /// </summary>
       [Display(Name ="手機號")]
       [MaxLength(22)]
       [Column(TypeName="nvarchar(22)")]
       [Required(AllowEmptyStrings=false)]
       public string PhoneNo { get; set; }

       
    }
}