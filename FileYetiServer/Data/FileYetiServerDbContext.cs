using System;
using System.Collections.Generic;
using System.Text;
using FileYetiServer.Models;
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
