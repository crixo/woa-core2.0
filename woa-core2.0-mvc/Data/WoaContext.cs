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
        public DbSet<AnamnesiRemota> AnamnesiRemote { get; set; }
        public DbSet<TipoAnamnesi> TipoAnamnesi { get; set; }
        public DbSet<Consulto> Consulti { get; set; }
        public DbSet<AnamnesiProssima> AnamnesiProssime { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Paziente>().ToTable("paziente");
            modelBuilder.Entity<Provincia>().ToTable("lkp_provincia");

            modelBuilder.Entity<AnamnesiRemota>().ToTable("anamnesi_remota");
            modelBuilder.Entity<TipoAnamnesi>().ToTable("lkp_anamnesi");

            modelBuilder.Entity<Consulto>().ToTable("consulto");
            modelBuilder.Entity<AnamnesiProssima>().ToTable("anamnesi_prossima");
        }
    }
}