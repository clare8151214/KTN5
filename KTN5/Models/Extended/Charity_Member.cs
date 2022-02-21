using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KTN5.Models
{
    [MetadataType(typeof(Charity_MemberMetadata))]
    public partial class Charity_Member
    {
        
    }
    public class Charity_MemberMetadata
    {
        [DisplayName("單位名稱")]
        public string c_name { get; set; }
        [DisplayName("單位地址")]
        public string c_address { get; set; }
        [DisplayName("單位連絡電話")]
        public string c_phone { get; set; }
        [DisplayName("單位負責人姓名")]
        public string c_pname { get; set; }
        [DisplayName("公益單位圖片")]
        public string photo { get; set; }
        [DisplayName("愛心碼")]
        public Nullable<int> heart_code { get; set; }
    }
}