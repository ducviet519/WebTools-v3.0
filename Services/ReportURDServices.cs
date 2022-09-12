using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Context;
using WebTools.Models;
using WebTools.Services.Interface;

namespace WebTools.Services
{
    public class ReportURDServices : IReportURDServices
    {
        private readonly DatabaseContext _context;

        public ReportURDServices(DatabaseContext context)
        {
            _context = context;
        }
        public List<ReportURD> GetReportList()
        {
            throw new NotImplementedException();
        }
    }
}
