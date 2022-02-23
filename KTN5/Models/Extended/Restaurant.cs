using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KTN5.Models
{
    [MetadataType(typeof(RestaurantMetadata))]
    public partial class Restaurant
    {

    }

    public class RestaurantMetadata
    {
        public int rId { get; set; }
        [DisplayName("餐廳")]
        public string rName { get; set; }
        [DisplayName("電話")]
        public string rPhone { get; set; }
        [DisplayName("地址")]
        public string rAddress { get; set; }
        [DisplayName("開店時間")]
        public Nullable<System.TimeSpan> startTime { get; set; }
        
        [DisplayName("結束時間")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{HH:mm}")]
        public Nullable<System.TimeSpan> endTime { get; set; }
    }
}