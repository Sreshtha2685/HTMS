//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class RoomRate
    {
        public int id { get; set; }
        public string Rate_Name { get; set; }
        public int room_TypeId { get; set; }
        public int room_Rate_TypeId { get; set; }
        public string Description { get; set; }
    
        public virtual RoomRateType RoomRateType { get; set; }
        public virtual RoomType RoomType { get; set; }
    }
}