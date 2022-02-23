using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KTN5.Models
{
    [MetadataType(typeof(ShoppingCartMetadata))]
    public partial class ShoppingCart
    {
    }
    public class ShoppingCartMetadata
    {

        [DisplayName("物資名稱")]
        public string oName { get; set; }
        [DisplayName("物資編號")]
        public int oId { get; set; }
        [DisplayName("物資介紹")]
        public string oIntro { get; set; }
        [DisplayName("捐贈數量")]
        public Nullable<int> oQty { get; set; }
    }

}