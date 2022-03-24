using Microsoft.EntityFrameworkCore;
using TiendaOnline.Web.Models;

namespace TiendaOnline.Web.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options){}

        public DbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Country>()
                .HasIndex(t => t.name)
                .IsUnique();

        }
    }
}
