using Microsoft.EntityFrameworkCore;
using liquidacion_ajoo.Models;

namespace liquidacion_ajoo
{
    public class ApplicationDbcontext : DbContext
    {
        public ApplicationDbcontext(DbContextOptions<ApplicationDbcontext> options) : base(options)
        {
        }

        public DbSet<Pagos_ajoo> pagos_ajoos {  get; set; }
        public DbSet<Clientes_ajoo> clientes_ajoos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pagos_ajoo>().ToTable("Pagos_ajoo"); 
            modelBuilder.Entity<Clientes_ajoo>().ToTable("Clientes_ajoo");
            base.OnModelCreating(modelBuilder); 
        }

    }

}
