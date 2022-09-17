using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using WebTools.Context;

namespace WebTools.Models
{
    public class ReportListViewModel
    {
        public ReportList ReportList { get; set; }

        public List<ReportList> ReportLists { get; set; }

        public PagingList<ReportList> PagingLists { get; set; }

    }
}
