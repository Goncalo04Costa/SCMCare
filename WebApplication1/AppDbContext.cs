using Microsoft.EntityFrameworkCore;
using Modelos;
using Models;
using ObjetosNegocio;

namespace WebApplication1
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Sobremesa> Sobremesas { get; set; }
        public DbSet<Alta> Altas { get; set; }
        public DbSet<Avaliacao> Avaliacoes { get; set; }
        public DbSet<Avaria> Avarias { get; set; }
        public DbSet<Cama> Camas { get; set; }
        public DbSet<Consulta> Consultas { get; set; }
        public DbSet<Equipamento> Equipamentos { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }

        public DbSet<Horario> Horarios { get; set; }

        public DbSet<Hospital> Hospitais { get; set; }

        public DbSet<Sopas> Sopa { get; set; }

    }
}



