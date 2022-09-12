using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebTools.Models
{
    public class ReportURD
    {
        [Key]
        public int IdURD { get; set; }
        public string Des { get; set; }
        public bool IsChecked { get; set; }
    }
}
