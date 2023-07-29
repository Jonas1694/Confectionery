using Confectionery.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Confectionery.Data
{
    public class DataContext:IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }
        public DbSet<Cliente> Clientes { get; set; }
		public DbSet<Facturacion> Facturacion { get; set; }
		public DbSet<DetalleFacturas> DetalleFacturas { get; set; }
		public DbSet<TipoDocumento> TipoDocumentos { get; set; }
		public DbSet<Productos> Productos { get; set; }
	}
}
