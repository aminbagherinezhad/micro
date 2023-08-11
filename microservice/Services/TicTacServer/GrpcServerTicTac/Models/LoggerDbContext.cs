using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpcServerTicTac.Models
{
    public class LoggerDbContext : DbContext
    {
        public LoggerDbContext()
        {
        }

        public LoggerDbContext(DbContextOptions<LoggerDbContext> options)
            : base(options)
        {
        }
         public virtual DbSet<LogMessageReciver> LogMessageRecivers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=.;Database=LoggerDb;Trusted_Connection=True;TrustServerCertificate=True");

    }
}
