using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HTMS.Models
{
    public class UserModel
    {
        [HiddenInput(DisplayValue = false)]
        public int id { get; set; }
       
        public string UserName { get; set; }
        public string Password { get; set; }

        public string UserRole { get; set; }

        public string Description { get; set; }

        //public int InsertedBy { get; set; }
        //public DateTime InsertedOn { get; set; }
        //public bool IsActive { get; set; }
        //public bool IsDelete { get; set; }
    }
}