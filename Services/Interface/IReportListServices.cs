using System.Collections.Generic;
using System.Threading.Tasks;
using WebTools.Models;

namespace WebTools.Services
{
    public interface IReportListServices
    {
        public Task<List<ReportList>> GetReportListAsync();

        public Task<ReportList> GetReportByIDAsync(string id);
        public Task<string> InsertReportListAsync(ReportList reportList);

        public Task<string> UpdateReportListAsync(ReportList reportList);
    }
}
