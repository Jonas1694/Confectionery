using Confectionery.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Confectionery.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }
        public DbSet<Cliente> Clientes { get; set; }
    }
}
