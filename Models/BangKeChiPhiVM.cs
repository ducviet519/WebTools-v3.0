using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using WebTools.Models.Entities;

namespace WebTools.Models
{
    public class BangKeChiPhiVM
    {
        public List<BangKeChiPhi> bangKeChiPhis { get; set; }
        public BangKeChiPhi bangKeChiPhi { get; set; }
        public IEnumerable<SelectListItem> Depts { get; set; }
    }
}
