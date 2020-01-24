using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HTMS.Models
{
    public class CountryModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        public string CountryName { get; set; }

        public string Description { get; set; }

        //public int InsertedBy { get; set; }
        //public DateTime InsertedOn { get; set; }
        //public bool IsActive { get; set; }
        //public bool IsDelete { get; set; }

    }
}