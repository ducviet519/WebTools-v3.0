using System.Collections.Generic;
using System.Threading.Tasks;
using WebTools.Models;

namespace WebTools.Services
{
    public interface IReportDetailServices
    {
        public Task<List<ReportDetail>> GetReportDetailAsync(string IdBieuMau);

        public Task<string> InsertReportDetailAsync(ReportDetail reportDetail);
    }
}
