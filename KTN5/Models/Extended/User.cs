using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace KTN5.Models
{
    [MetadataType(typeof(UserMetadata))]
    public partial class User
    {
        public string ConfirmPassword { get; set; }
    }
    public class UserMetadata
    {

        [Display(Name = "姓名")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "姓名必填")]
        public string name { get; set; }

        [Display(Name = "帳號")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "帳號必填")]
        [DataType(DataType.EmailAddress)]
        public string account { get; set; }

        [Display(Name = "密碼")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "密碼必填")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "至少輸入六個字")]
        public string password { get; set; }

        [Display(Name = "確認密碼")]
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage = "兩組密碼不匹配")]
        public string ConfirmPassword { get; set; }


    }
}