using Microsoft.EntityFrameworkCore;
using Models;

namespace WebApplication1.Conecta
{
    public class SCMDbContext : DbContext
    {
        public DbSet<Sobremesa> Sobremesas { get; set; }

        public SCMDbContext(DbContextOptions<SCMDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuração da entidade Sobremesa
            modelBuilder.Entity<Sobremesa>(entity =>
            {
                entity.ToTable("Sobremesas"); // Nome da tabela no banco de dados
                entity.HasKey(e => e.Id); // Define a chave primária
                entity.Property(e => e.Nome).IsRequired(); // Define a propriedade Nome como obrigatória
            });
        }
    }
}
