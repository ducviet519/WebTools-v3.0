using System.Collections.Generic;
using WebTools.Models;

namespace WebTools.Services
{
    public interface IReportSoftServices
    {
        public List<ReportSoft> GetReportSoft(string IdBieuMau);

        public string InsertReportSoft(ReportSoft reportSoft);
        public string UpdateReportSoft(ReportSoft reportSoft);
        public string DeleteReportSoft(string IdBieuMau);
    }
}
