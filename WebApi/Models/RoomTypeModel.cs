using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class RoomTypeModel
    {
        public int id { get; set; }
        public string RoomName { get; set; }
        public Nullable<int> Max_Adult_No { get; set; }
        public Nullable<int> Max_Child_No { get; set; }
        public string Code { get; set; }
        public Nullable<int> BedId { get; set; }
        public string Description { get; set; }
        public Nullable<int> RoomStatusId { get; set; }
        public Nullable<System.DateTime> InsertedOn { get; set; }
        public Nullable<int> InsertedBy { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    }
}