using System.Collections.Generic;
using WebTools.Models;

namespace WebTools.Services
{
    public interface IReportDetailServices
    {
        public List<ReportDetail> GetReportDetail(string IdBieuMau);

        public string InsertReportDetail(ReportDetail reportDetail);
    }
}
