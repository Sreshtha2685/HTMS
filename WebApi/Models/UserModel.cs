using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string UserRole{ get; set; }
        public string UserName { get; set; }
        public string Description { get; set; }
        public int InsertedBy { get; set; }
        public DateTime InsertedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
    }
}