using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KTN5.Models
{

    [MetadataType(typeof(FundMetadata))]
    public partial class Fund
    {
        public HttpPostedFileBase photo { get; set; }
    }
    public class FundMetadata
    {
        [DisplayName("專案概述")]
        public string fText { get; set; }
        [DisplayName("目標金額")]
        public Nullable<decimal> targetMoney { get; set; }
        [DisplayName("累積金額")]
        public Nullable<decimal> accMoney { get; set; }
        [DisplayName("專案開始日期")]
        public Nullable<System.DateTime> startTime { get; set; }
        [DisplayName("專案結束日期")]
        public Nullable<System.DateTime> endTime { get; set; }
        [DisplayName("聯絡窗口姓名")]
        public string trueName { get; set; }
        [DisplayName("聯絡電話")]
        public string fPhone { get; set; }
        [DisplayName("電子郵件")]
        public string fEmail { get; set; }
        public string fPhoto { get; set; }
        [DisplayName("專案名稱")]
        public string fName { get; set; }
    }
}