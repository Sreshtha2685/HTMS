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
        [HiddenInput(DisplayValue = false)]
        public int InsertedBy { get; set; }

        [HiddenInput(DisplayValue = false)]
        public DateTime InsertedOn { get; set; }
        [HiddenInput(DisplayValue = false)]
        public bool IsActive { get; set; }
        [HiddenInput(DisplayValue = false)]
        public bool IsDelete { get; set; }
    }
}