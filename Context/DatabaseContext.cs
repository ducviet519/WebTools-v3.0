using Microsoft.EntityFrameworkCore;
using WebTools.Models;

namespace WebTools.Context
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {

        }

        public virtual DbSet<ReportList> ReportLists { get; set; }
    }
}
