using System.Collections.Generic;
using WebTools.Models;

namespace WebTools.Services
{
    public interface IReportDetailServices
    {
        public List<ReportDetail> GetReportDetail();

        public string InsertReportDetail(ReportDetail reportDetail);
        public string UpdateReportDetail(ReportDetail reportDetail);
        public string DeleteReportDetail(string IdBieuMau);
    }
}
