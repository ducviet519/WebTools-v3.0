using System.Collections.Generic;
using System.Threading.Tasks;
using WebTools.Models;

namespace WebTools.Services
{
    public interface IReportSoftServices
    {
        public Task<List<ReportSoft>> GetReportSoftAsync(string IdBieuMau);

        public Task<string> InsertReportSoftAsync(ReportSoft reportSoft);
    }
}
