using System.Collections.Generic;
using WebTools.Context;
using WebTools.Models.Entity;

namespace WebTools.Models
{
    public class ReportListViewModel
    {
        public ReportList ReportList { get; set; }

        public List<ReportList> ReportLists { get; set; }

        public FileDetails Files { get; set; }
        public PagingList<ReportList> PagingLists { get; set; }
    }
}
