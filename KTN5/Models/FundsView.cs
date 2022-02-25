using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace KTN5.Models
{
    public class FundsView
    {
        public int fId { get; set; }
        [DisplayName("提案者")]
        public string cName { get; set; }
        [DisplayName("提案名稱")]
        public string fName { get; set; }
        [DisplayName("累計金額")]
        public Nullable<decimal> accMoney { get; set; }
        [DisplayName("開始時間")]
        public Nullable<System.DateTime> startTime { get; set; }
        [DisplayName("結束時間")]
        public Nullable<System.DateTime> endTime { get; set; }
        [DisplayName("倒數")]
        public TimeSpan? countDown { get; set; }
        [DisplayName("進度")]
        public decimal? progress { get; set; }
        public string fPhoto { get; set; }
    }
}