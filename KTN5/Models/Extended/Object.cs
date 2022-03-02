using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KTN5.Models
{
    
    [MetadataType(typeof(ObjectMetadata))]
    public partial class Object
    {
        public HttpPostedFileBase photo { get; set; }
    }
    public class ObjectMetadata
    {
        [DisplayName("物資編號")]
        public int oId { get; set; }
        [DisplayName("物資名稱")]
        [Required(ErrorMessage = "物資名稱不可空白")]
        public string oName { get; set; }
        [DisplayName("需求公益單位")]
        [Required(ErrorMessage = "公益單位不可空白")]
        public Nullable<int> cId { get; set; }
        [DisplayName("物資照片")]
        public string oPhoto { get; set; }
        public HttpPostedFileBase photo { get; set; }

        [DisplayName("需求數量")]
        public string oNumber { get; set; }
        [DisplayName("需求說明")]
        [Required(ErrorMessage = "概述不可空白")]
        public string oIntro { get; set; }
        [DisplayName("物資類別")]
        public string type { get; set; }

    }
}