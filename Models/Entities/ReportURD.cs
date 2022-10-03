using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebTools.Models
{
    public class ReportURD
    {
        [Key]
        public int ID { get; set; }
        public string Des { get; set; }

        [NotMapped]
        public bool IsChecked { get; set; }

        public string Value { get; set; }
        public string Text { get; set; }
    }

}
