using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTMS.Models
{
    public class BedModel
    {
        public Nullable<int>BedNo { get; set; }

        public Nullable<int> BedCode{ get; set; }

        public string Description { get; set; }
    }
}