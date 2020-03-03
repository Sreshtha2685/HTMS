using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTMS.Models
{
    public class GuestModel
    {
        public int Id { get; set; }
        public string GuestName { get; set; }
        public Nullable<int> GuestContactNumber { get; set; }
        public string GuestAddress { get; set; }
        public string IdProof { get; set; }
        public string Description { get; set; }
        public Nullable<int> RoomId { get; set; }
        public Nullable<int> ServiceTypeId { get; set; }
        public Nullable<int> InsertedBy { get; set; }
        public Nullable<System.DateTime> InsertedOn { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    }
}