using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models;

namespace WebTools.Services
{
    public interface IReportURDServices
    {
        public Task<List<ReportURD>> GetAll_URDAsync();
    }
}
