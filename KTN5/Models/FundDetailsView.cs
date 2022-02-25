using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace KTN5.Models
{
    public class FundDetailsView
    {
        public int fId { get; set; }
        [DisplayName("提案者")]
        public string cName { get; set; }
        [DisplayName("提案名稱")]
        public string fName { get; set; }
        [DisplayName("目標金額")]
        public Nullable<decimal> targetMoney { get; set; }
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
        [DisplayName("專案內容")]
        public string fText { get; set; }
        public string fPhoto { get; set; }
        public int sId { get; set; }
        [DisplayName("方案金額")]
        public Nullable<decimal> sMoney { get; set; }
        [DisplayName("方案介紹")]
        public string intro { get; set; }
        [DisplayName("方案名稱")]
        public string sName { get; set; }

        public string sPhoto { get; set; }
    }
}