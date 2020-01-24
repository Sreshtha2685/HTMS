using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HTMS.Models
{
    public class HotelModel
    {
        [HiddenInput(DisplayValue = false)]
        public int id { get; set; }
        public string HotelName { get; set; }

        public Nullable<int> HotelNumber { get; set; }

        public string Description { get; set; }
    }
}