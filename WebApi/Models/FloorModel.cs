﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class FloorModel
    {
        public int Id { get; set; }
        public Nullable<int> Floor_Number { get; set; }
      
        public string Description { get; set; }
        public int InsertedBy { get; set; }
        public DateTime InsertedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
    }
}