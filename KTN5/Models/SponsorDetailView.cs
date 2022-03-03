using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace KTN5.Models
{
    public class SponsorDetailView
    {
        [DisplayName("贊助專案")]
        public string fName { get; set; }
        [DisplayName("贊助方案")]
        public string sName { get; set; }
        [DisplayName("總金額")]
        public decimal money { get; set; }
        [DisplayName("贊助時間")]
        public DateTime created_at { get; set; }

    }
}