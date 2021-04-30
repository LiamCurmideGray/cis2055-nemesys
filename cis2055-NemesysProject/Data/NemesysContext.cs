using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cis2055_NemesysProject.Models;

namespace cis2055_NemesysProject.Data
{
    public class NemesysContext : DbContext
    {
        public NemesysContext (DbContextOptions<NemesysContext> options)
            : base(options)
        { }

        public DbSet<Roles> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Roles>().ToTable("Roles");
            // modelBuilder.Entity<Reporter>().ToTable("Reporter");
            // modelBuilder.Entity<Investigator>().ToTable("Investigator");
            // modelBuilder.Entity<Report>().ToTable("Report");
            // modelBuilder.Entity<Investigation>().ToTable("Investigation");
        }
    }

}
