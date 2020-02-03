using FileYetiServer.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FileYetiServer.Data
{
    public class FileYetiServerDbContext : DbContext
    {
        public FileYetiServerDbContext()
        {
            Database.EnsureCreated();
        }
        public DbSet<TransferJob> TransferJobs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=fileYetiServer.db");
        }
    }
}
