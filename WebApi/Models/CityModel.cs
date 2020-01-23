using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class CityModel
    {
        public int Id { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string Description { get; set; }
        public int InsertedBy { get; set; }
        public DateTime InsertedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
    }
}
