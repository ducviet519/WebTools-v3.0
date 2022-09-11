using System.Collections.Generic;
using WebTools.Models;

namespace WebTools.Services
{
    public interface IReportVersionServices
    {
        public List<ReportVersion> GetReportVersion();

        public string InsertReportVersion(ReportVersion reportVersion);
        public string UpdateReportVersion(ReportVersion reportVersion);
        public string DeleteReportVersion(string IdBieuMau);
    }
}
