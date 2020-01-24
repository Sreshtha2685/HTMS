using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HTMS.Models
{
    public class StateModel
    {
        [HiddenInput(DisplayValue = false)]
        public int id { get; set; }
       // public Nullable<int> CityId { get; set; }
       // public string CityName { get; set; }
       public string CityName { get; set; }

        public string StateName { get; set; }

        public string Description { get; set; }

        //public int InsertedBy { get; set; }
        //public DateTime InsertedOn { get; set; }
        //public bool IsActive { get; set; }
        //public bool IsDelete { get; set; }
    }
}