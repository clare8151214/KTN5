using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KTN5.Models
{

    [MetadataType(typeof(SolutionMetadata))]
    public partial class Solution
    {
       public HttpPostedFileBase photo { get; set; }

    }
    public class SolutionMetadata
    {
        public int sId { get; set; }
        [DisplayName("專案編號")]
        public int fId { get; set; }
        [DisplayName("方案金額")]
        public Nullable<decimal> sMoney { get; set; }
        [DisplayName("方案介紹")]
        public string intro { get; set; }
        [DisplayName("方案名稱")]
        public string sName { get; set; }
        public string sPhoto { get; set; }
    }

}