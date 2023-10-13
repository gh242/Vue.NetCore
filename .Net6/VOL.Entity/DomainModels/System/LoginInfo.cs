using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace VOL.Entity.DomainModels
{
    public class LoginInfo
    {


        [Display(Name = "用戶名")]
        [MaxLength(50)]
        [Required(ErrorMessage = "用戶名不能為空")]
        public string UserName { get; set; }
        [MaxLength(50)]
        [Display(Name = "密碼")]
        [Required(ErrorMessage = "密碼不能為空")]
        public string Password { get; set; }
        [MaxLength(6)]
        [Display(Name = "驗證碼")]
        [Required(ErrorMessage = "驗證碼不能為空")]
        public string VerificationCode { get; set; }
        [Required(ErrorMessage = "參數不完整")]
        /// <summary>
        /// 2020.06.12增加驗證碼
        /// </summary>
        public string UUID { get; set; }
    }
}
