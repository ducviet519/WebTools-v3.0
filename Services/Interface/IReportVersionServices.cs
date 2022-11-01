using System.Collections.Generic;
using System.Threading.Tasks;
using WebTools.Models;

namespace WebTools.Services
{
    public interface IReportVersionServices
    {
        public Task<List<ReportVersion>> GetReportVersionAsync(string IdBieuMau);

        public Task<string> InsertReportVersionAsync(ReportVersion reportVersion);

        public Task<string> UpdateReportVersionAsync(ReportVersion reportVersion);

        public Task<string> DeleteReportVersionAsync(string IDPhienBan);

    }
}
