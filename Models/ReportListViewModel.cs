using System.Collections.Generic;
using WebTools.Context;

namespace WebTools.Models
{
    public class ReportViewModel
    {
        public ReportList ReportList { get; set; }

        public List<ReportList> ReportLists { get; set; }       
        
        public PagingList<ReportList> PagingLists { get; set; }
    }
}
