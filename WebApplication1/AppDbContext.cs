using Microsoft.EntityFrameworkCore;
using Modelos;
using Models;


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
        public DbSet<Sopa> Sopa { get; set; }
        public DbSet<Limpeza> Limpezas { get; set; }
        public DbSet<Material> Materiais { get; set; }
        public DbSet<Medicamento> Medicamentos { get; set; }
        public DbSet<Mensalidade> Mensalidades { get; set; } 
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Plano> Planos { get; set; }
        public DbSet<Prato> Pratos { get; set; }
        public DbSet<Prescricao> Prescricoes { get; set; }
        public DbSet<Quarto> Quartos { get; set; }

        public DbSet<Responsavel> Responsaveis { get; set; }

        public DbSet<Senha> Senhas { get; set; }
    }
}
