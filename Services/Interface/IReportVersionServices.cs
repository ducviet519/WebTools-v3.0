using System.Collections.Generic;
using WebTools.Models;

namespace WebTools.Services
{
    public interface IReportVersionServices
    {
        public List<ReportVersion> GetReportVersion(string IdBieuMau);

        public string InsertReportVersion(ReportVersion reportVersion);
    }
}
