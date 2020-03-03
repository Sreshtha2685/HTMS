using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTMS.Models
{
    public class RoomModel
    {
        public int id { get; set; }
        public Nullable<int> RoomTypeId { get; set; }
        public Nullable<int> FloorId { get; set; }
        public Nullable<int> HotelId { get; set; }
        public Nullable<int> BedId { get; set; }
        public Nullable<int> RoomNumber { get; set; }
        public Nullable<int> RoomStatusId { get; set; }
        public string Description { get; set; }
        public Nullable<int> InsertedBy { get; set; }
        public Nullable<System.DateTime> InsertedOn { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    }
}