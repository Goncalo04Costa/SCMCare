using Microsoft.EntityFrameworkCore;
using Models;
using System;

namespace WebApplication1.Conecta
{
    public class SCMDbContext: DbContext
    {
        public SCMDbContext(DbContextOptions<SCMDbContext> options) : base(options)
        {
        }

        // DbSet properties for your entities
        public DbSet<Sobremesa> Sobremesas { get; set; }
    }
}
