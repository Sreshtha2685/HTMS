using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class BedModel
    {
        public int id { get; set; }
        public int BedNumber { get; set; }

        public int BedCode{ get; set; }

        public string Description { get; set; }
    }
}