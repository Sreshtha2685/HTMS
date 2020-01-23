using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HTMS.Models
{
    public class AutoGenerate
    {
        [Key]
        public string IdName { get; set; }

        [Required]
        public int Id { get; set; }

    }
}