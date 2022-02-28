using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace KTN5.Models
{
    public class ObjectIndexView
    {
        public int oId { get; set; }
        [DisplayName("物資名稱")]
        public string oName { get; set; }
        [DisplayName("物資照片")]
        public string oPhoto { get; set; }

        [DisplayName("需求數量")]
        public string oNumber { get; set; }
        [DisplayName("需求說明")]
        public string oIntro { get; set; }
        [DisplayName("物資類別")]
        public string type { get; set; }
        public string c_name { get; set; }
        
    }
}