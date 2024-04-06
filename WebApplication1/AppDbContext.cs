using Microsoft.EntityFrameworkCore;
using Modelos;

namespace WebApplication1
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Sobremesa> Sobremesas { get; set; }
    }
}
