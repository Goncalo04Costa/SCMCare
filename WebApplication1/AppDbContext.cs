using Microsoft.EntityFrameworkCore;
using Modelos;


namespace WebApplication1
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ContaCorrenteMaterial> ContaCorrenteMateriais { get; set; }

        public DbSet<ContaCorrenteMedicamento> ContaCorrenteMedicamentos { get; set; }

        public DbSet<Alta> Alta { get; set; }

        public DbSet<Avaliacao> Avaliacoes { get; set; }

        public DbSet<Avaria> Avarias { get; set; }

        public DbSet<Cama> Camas { get; set; }

        public DbSet<Consulta> Consultas { get; set; }

        public DbSet<ContactoFornecedor> ContactosFornecedores { get; set; }

        public DbSet<ContactoFuncionario> ContactosFuncionarios { get; set; }

        public DbSet<ContactoResponsavel> ContactosResponsaveis { get; set; }

        public DbSet<Equipamento> Equipamentos { get; set; }

        public DbSet<FeriasFuncionario> FeriasFuncionario { get; set; }

        public DbSet<Fornecedor> Fornecedores { get; set; }

        public DbSet<FornecedorMedicamento> FornecedoresMedicamento { get; set; }

        public DbSet<Funcionario> Funcionarios { get; set; }

        public DbSet<Horario> Horarios { get; set; }

        public DbSet<Hospital> Hospitais { get; set; }

        public DbSet<Limpeza> Limpezas { get; set; }

        public DbSet<Material> Materiais { get; set; }

        public DbSet<Medicamento> Medicamentos { get; set; }

        public DbSet<MedicamentoPrescricao> MedicamentosPrescricao { get; set; }

        public DbSet<Mensalidade> Mensalidades { get; set; }

        public DbSet<Menu> Menu { get; set; }

        public DbSet<Notificacao> Notificacoes { get; set; }

        public DbSet<NotificacaoFuncionario> NotificacoesFuncionario { get; set; }

        public DbSet<NotificacaoResponsavel> NotificacoesResponsavel { get; set; }

        public DbSet<PedidoMaterial> PedidosMaterial { get; set; }

        public DbSet<PedidoMedicamento> PedidosMedicamento { get; set; }

        public DbSet<Plano> Planos { get; set; }

        public DbSet<Prato> Pratos { get; set; }

        public DbSet<Prescricao> Prescricoes { get; set; }

        public DbSet<Quarto> Quartos { get; set; }

        public DbSet<Responsavel> Responsaveis { get; set; }

        public DbSet<Senha> Senhas { get; set; }

        public DbSet<Sessao> Sessoes { get; set; }

        public DbSet<Sobremesa> Sobremesas { get; set; }

        public DbSet<Sopa> Sopas { get; set; }

        public DbSet<TipoAdmissao> TiposAdmissao { get; set; }

        public DbSet<TipoAlergia> TiposAlergia { get; set; }

        public DbSet<TipoAvaliacao> TipoAvaliacao { get; set; }

        public DbSet<TipoContacto> TipoContacto { get; set; }

        public DbSet<TipoEquipamento> TiposEquipamento { get; set; }

        public DbSet<TipoFuncionario> TiposFuncionario { get; set; }

        public DbSet<TipoMaterial> TiposMaterial { get; set; }

        public DbSet<TipoPagamento> TiposPagamento { get; set; }

        public DbSet<TipoQuarto> TiposQuarto { get; set; }

        public DbSet<Turno> Turnos { get; set; }

        public DbSet<Utente> Utentes { get; set; }

        public DbSet<UtenteAlergia> UtentesAlergias { get; set; }

        public DbSet<UserResponsavel> UserResponsavel { get; set; }

        public DbSet<UserFuncionario> UserFuncionario { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Modelação para entidades sem chave primária própria

            modelBuilder.Entity<Alta>().HasKey(c => new { c.UtentesId });

            modelBuilder.Entity<ContactoFornecedor>().HasKey(c => new { c.FornecedoresId, c.TipoContactoId });

            modelBuilder.Entity<ContactoFuncionario>().HasKey(c => new { c.FuncionariosId, c.TipoContactoId });

            modelBuilder.Entity<ContactoResponsavel>().HasKey(c => new { c.ResponsaveisId, c.TipoContactoId });

            modelBuilder.Entity<FornecedorMedicamento>().HasKey(c => new { c.MedicamentosId, c.FornecedoresId });

            modelBuilder.Entity<Horario>().HasKey(c => new { c.FuncionariosId, c.TurnosId, c.Dia });

            modelBuilder.Entity<MedicamentoPrescricao>().HasKey(c => new { c.PrescricoesId, c.MedicamentosId });

            modelBuilder.Entity<Mensalidade>().HasKey(c => new { c.Mes, c.UtentesId });

            modelBuilder.Entity<NotificacaoFuncionario>().HasKey(c => new { c.FuncionarioId, c.NotificacaoId });

            modelBuilder.Entity<NotificacaoResponsavel>().HasKey(c => new { c.ResponsavelId, c.NotificacaoId });

            modelBuilder.Entity<Senha>().HasKey(c => new { c.FuncionariosId, c.MenuId });

            modelBuilder.Entity<UtenteAlergia>().HasKey(c => new { c.UtentesId, c.TiposAlergiaId });

            modelBuilder.Entity<UserFuncionario>().HasKey(c => new { c.FuncionariosId });

            modelBuilder.Entity<UserResponsavel>().HasKey(c => new { c.ResponsaveisId });
        }
    }
}
