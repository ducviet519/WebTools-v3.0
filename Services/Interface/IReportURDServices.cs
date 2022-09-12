using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models;

namespace WebTools.Services.Interface
{
    interface IReportURDServices
    {
        public List<ReportURD> GetReportList();
    }
}
