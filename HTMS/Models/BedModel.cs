using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HTMS.Models
{
    public class BedModel
    {
        [HiddenInput(DisplayValue = false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public Nullable<int> Bed_Number { get; set; }

        public Nullable<int> Bed_Code { get; set; }

        public string Description { get; set; }
        public int InsertedBy { get; set; }
        public DateTime InsertedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
    }
}