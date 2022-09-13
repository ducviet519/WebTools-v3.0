using Microsoft.EntityFrameworkCore;
using WebTools.Models;

namespace WebTools.Context
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {

        }

        public virtual DbSet<ReportList> Reports { get; set; }
        public virtual DbSet<ReportVersion> Versions { get; set; }
        public virtual DbSet<ReportSoft> Softs { get; set; }
        public virtual DbSet<ReportDetail> Details { get; set; }
        public virtual DbSet<ReportURD> URDs { get; set; }
    }
}
