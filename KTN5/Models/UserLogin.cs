using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KTN5.Models
{
    public class UserLogin
    {
        [Display(Name = "帳號")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "帳號必填")]
        public string account { get; set; }

        [Display(Name = "密碼")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "密碼必填")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Display(Name = "記住我")]
        public bool RememberMe { get; set; }

    }
}