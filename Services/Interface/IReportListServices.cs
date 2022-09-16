using System.Collections.Generic;
using WebTools.Models;

namespace WebTools.Services
{
    public interface IReportListServices
    {
        public List<ReportList> GetReportList();

        public string InsertReportList(ReportList reportList);
    }
}
