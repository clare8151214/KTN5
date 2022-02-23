using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KTN5.Models
{
    [MetadataType(typeof(OrderMetadata))]
    public partial class Order
    {

    }
    public class OrderMetadata
    {
        [DisplayName("捐贈ID")]
        public int orderId { get; set; }
        [DisplayName("捐贈者姓名")]
        public string hName { get; set; }

        [DisplayName("連絡電話")]
        public string hPhone { get; set; }
        [DisplayName("電子郵件")]
        public string hEamil { get; set; }
        [DisplayName("運送方式")]
        public string hCarrier { get; set; }
        [DisplayName("捐贈日期")]
        public Nullable<System.DateTime> created_at { get; set; }
    }
}