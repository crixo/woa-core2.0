using Woa.Models;
using Microsoft.EntityFrameworkCore;

namespace Woa.Data
{
    public class WoaContext : DbContext
    {
        public WoaContext(DbContextOptions<WoaContext> options) : base(options)
        {
        }

        public DbSet<Paziente> Pazienti { get; set; }

        public DbSet<Provincia> Province { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Paziente>().ToTable("paziente");
            modelBuilder.Entity<Provincia>().ToTable("lkp_provincia");
        }
    }
}