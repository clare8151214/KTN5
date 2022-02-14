using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KTN5.Models
{
    public class ResetPasswordModel
    {
        [DisplayName("新密碼")]
        [Required(ErrorMessage = "新密碼必填", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DisplayName("確認新密碼")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "與新密碼不符合")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string ResetCode { get; set; }
    }
}