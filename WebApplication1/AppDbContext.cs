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

        public DbSet<Sopa> Sopas { get; set; }

        public DbSet<Utente> Utentes { get; set; }

        public DbSet<UtenteAlergia> UtenteAlergias { get; set; }

        public DbSet<UserResponsavel> UsersResponsavel { get; set; }

        public DbSet<UserFuncionario> UsersFuncionario { get; set; }

        public DbSet<Turno> Turnos { get; set; }

        public DbSet<TipoQuarto> TiposQuarto { get; set; }

        public DbSet<TipoPagamento> TiposPagamento { get; set; }

        public DbSet<TipoMaterial> TiposMaterial { get; set; }

        public DbSet<TipoFuncionario> TiposFuncionario { get; set; }

        public DbSet<TipoEquipamento> TiposEquipamento { get; set; }

        public DbSet<TipoContacto> TiposContacto { get; set; }

        public DbSet<TipoAvaliacao> TiposAvaliacao { get; set; }

        public DbSet<TipoAlergia> TiposAlergia { get; set; }

        public DbSet<TipoAdmissao> TiposAdmissao { get; set; }

    }
}



