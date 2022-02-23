using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KTN5.Models
{
    [MetadataType(typeof(OrderDetailMetadata))]
    public partial class OrderDetail
    {

    }
    public class OrderDetailMetadata
    {
        [DisplayName("捐贈ID")]
        public int orderId { get; set; }
        public int odId { get; set; }
        [DisplayName("物資名稱")]
        public string oName { get; set; }
        [DisplayName("物資數量")]
        public Nullable<int> oQty { get; set; }
    }
}