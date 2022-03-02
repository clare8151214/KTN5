using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KTN5.Models
{
    public class CVMMember
    {
        public List<User> gmembers { get; set; }
        public List<Charity_Member> charities { get; set; }

        public List<Restaurant> restaurants { get; set; }

        public List<Object> objects { get; set; }
    }
}