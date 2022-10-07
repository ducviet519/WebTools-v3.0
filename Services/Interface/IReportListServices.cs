using System.Collections.Generic;
using WebTools.Models;

namespace WebTools.Services
{
    public interface IReportListServices
    {
        public List<ReportList> GetReportList();

        public ReportList GetReportByID(string id);
        public string InsertReportList(ReportList reportList);

        public string UpdateReportList(ReportList reportList);
    }
}
