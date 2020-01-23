using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTMS.Models
{
    public class CityModel
    {
       // public int id { get; set; }

        public string CityName { get; set; }
        public string StateName { get; set; }

        //public Nullable<int>StateId{ get; set; }
        //public Nullable<int> CountryId { get; set; }
        public string CountryName { get; set; }
        public string Description { get; set; }
        

        //public Nullable<bool> IsChecked { get; set; }

        //public Nullable<System.DateTime> InsertedOn { get; set; }
        //public Nullable<int> InsertedBy { get; set; }
        //public Nullable<bool> IsActive { get; set; }
        //public Nullable<bool> IsDelete { get; set; }
    }
}