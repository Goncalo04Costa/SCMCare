using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Altum> Alta { get; set; }

    public virtual DbSet<Avaliaco> Avaliacoes { get; set; }

    public virtual DbSet<Avaria> Avarias { get; set; }

    public virtual DbSet<Cama> Camas { get; set; }

    public virtual DbSet<Consulta> Consultas { get; set; }

    public virtual DbSet<ContaCorrenteMateriai> ContaCorrenteMateriais { get; set; }

    public virtual DbSet<ContaCorrenteMedicamento> ContaCorrenteMedicamentos { get; set; }

    public virtual DbSet<ContactosFornecedore> ContactosFornecedores { get; set; }

    public virtual DbSet<ContactosFuncionario> ContactosFuncionarios { get; set; }

    public virtual DbSet<ContactosResponsavei> ContactosResponsaveis { get; set; }

    public virtual DbSet<Equipamento> Equipamentos { get; set; }

    public virtual DbSet<FeriasFuncionario> FeriasFuncionarios { get; set; }

    public virtual DbSet<Fornecedore> Fornecedores { get; set; }

    public virtual DbSet<Funcionario> Funcionarios { get; set; }

    public virtual DbSet<Horario> Horarios { get; set; }

    public virtual DbSet<Hospitai> Hospitais { get; set; }

    public virtual DbSet<Limpeza> Limpezas { get; set; }

    public virtual DbSet<Materiai> Materiais { get; set; }

    public virtual DbSet<MateriaisPlano> MateriaisPlanos { get; set; }

    public virtual DbSet<Medicamento> Medicamentos { get; set; }

    public virtual DbSet<MedicamentosPrescricao> MedicamentosPrescricaos { get; set; }

    public virtual DbSet<Mensalidade> Mensalidades { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<Notificaco> Notificacoes { get; set; }

    public virtual DbSet<NotificacoesFuncionario> NotificacoesFuncionarios { get; set; }

    public virtual DbSet<NotificacoesResponsavel> NotificacoesResponsavels { get; set; }

    public virtual DbSet<PedidosMaterial> PedidosMaterials { get; set; }

    public virtual DbSet<PedidosMedicamento> PedidosMedicamentos { get; set; }

    public virtual DbSet<Plano> Planos { get; set; }

    public virtual DbSet<Prato> Pratos { get; set; }

    public virtual DbSet<Prescrico> Prescricoes { get; set; }

    public virtual DbSet<Quarto> Quartos { get; set; }

    public virtual DbSet<Responsavei> Responsaveis { get; set; }

    public virtual DbSet<Senha> Senhas { get; set; }

    public virtual DbSet<Sesso> Sessoes { get; set; }

    public virtual DbSet<Sobremesa> Sobremesas { get; set; }

    public virtual DbSet<Sopa> Sopas { get; set; }

    public virtual DbSet<TipoAvaliacao> TipoAvaliacaos { get; set; }

    public virtual DbSet<TipoContacto> TipoContactos { get; set; }

    public virtual DbSet<TipoSessao> TipoSessaos { get; set; }

    public virtual DbSet<TiposAdmissao> TiposAdmissaos { get; set; }

    public virtual DbSet<TiposAlergium> TiposAlergia { get; set; }

    public virtual DbSet<TiposEquipamento> TiposEquipamentos { get; set; }

    public virtual DbSet<TiposFuncionario> TiposFuncionarios { get; set; }

    public virtual DbSet<TiposMaterial> TiposMaterials { get; set; }

    public virtual DbSet<TiposPagamento> TiposPagamentos { get; set; }

    public virtual DbSet<TiposQuarto> TiposQuartos { get; set; }

    public virtual DbSet<Turno> Turnos { get; set; }

    public virtual DbSet<UserFuncionario> UserFuncionarios { get; set; }

    public virtual DbSet<UserResponsavel> UserResponsavels { get; set; }

    public virtual DbSet<Utente> Utentes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=GONCALO;Database=PDS;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Altum>(entity =>
        {
            entity.HasKey(e => e.UtentesId).HasName("PK__Alta__41D42AE4257D09FD");

            entity.Property(e => e.UtentesId).ValueGeneratedNever();
            entity.Property(e => e.Destino).IsUnicode(false);
            entity.Property(e => e.Motivo).IsUnicode(false);

            entity.HasOne(d => d.Funcionarios).WithMany(p => p.Alta)
                .HasForeignKey(d => d.FuncionariosId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKAlta512473");

            entity.HasOne(d => d.Utentes).WithOne(p => p.Altum)
                .HasForeignKey<Altum>(d => d.UtentesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKAlta875261");
        });

        modelBuilder.Entity<Avaliaco>(entity =>
        {
            entity.HasKey(e => new { e.UtentesId, e.FuncionariosId, e.Data }).HasName("PK__Avaliaco__8B59C51DD9139945");

            entity.Property(e => e.Data).HasColumnType("datetime");
            entity.Property(e => e.Analise).IsUnicode(false);
            entity.Property(e => e.AucultacaoCardiaca).IsUnicode(false);
            entity.Property(e => e.AuscultacaoPolmunar).IsUnicode(false);

            entity.HasOne(d => d.Funcionarios).WithMany(p => p.Avaliacos)
                .HasForeignKey(d => d.FuncionariosId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKAvaliacoes491816");

            entity.HasOne(d => d.TipoAvaliacao).WithMany(p => p.Avaliacos)
                .HasForeignKey(d => d.TipoAvaliacaoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKAvaliacoes973926");

            entity.HasOne(d => d.Utentes).WithMany(p => p.Avaliacos)
                .HasForeignKey(d => d.UtentesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKAvaliacoes854604");
        });

        modelBuilder.Entity<Avaria>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Avarias__3214EC07A22900B7");

            entity.Property(e => e.Descricao).IsUnicode(false);

            entity.HasOne(d => d.Equipamentos).WithMany(p => p.Avaria)
                .HasForeignKey(d => d.EquipamentosId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKAvarias720079");
        });

        modelBuilder.Entity<Cama>(entity =>
        {
            entity.HasKey(e => e.UtentesId).HasName("PK__Camas__41D42AE4E2DF1252");

            entity.Property(e => e.UtentesId).ValueGeneratedNever();

            entity.HasOne(d => d.Quartos).WithMany(p => p.Camas)
                .HasForeignKey(d => d.QuartosId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKCamas796850");

            entity.HasOne(d => d.Utentes).WithOne(p => p.Cama)
                .HasForeignKey<Cama>(d => d.UtentesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKCamas704933");
        });

        modelBuilder.Entity<Consulta>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Consulta__3214EC07357F8E9F");

            entity.Property(e => e.Descricao).IsUnicode(false);

            entity.HasOne(d => d.Funcionarios).WithMany(p => p.Consulta)
                .HasForeignKey(d => d.FuncionariosId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKConsultas553850");

            entity.HasOne(d => d.Hospitais).WithMany(p => p.Consulta)
                .HasForeignKey(d => d.HospitaisId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKConsultas441237");

            entity.HasOne(d => d.Responsaveis).WithMany(p => p.Consulta)
                .HasForeignKey(d => d.ResponsaveisId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKConsultas335483");

            entity.HasOne(d => d.Utentes).WithMany(p => p.Consulta)
                .HasForeignKey(d => d.UtentesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKConsultas191062");
        });

        modelBuilder.Entity<ContaCorrenteMateriai>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ContaCor__3214EC074CA5D4DB");

            entity.Property(e => e.Fatura)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Observacoes).IsUnicode(false);

            entity.HasOne(d => d.Funcionarios).WithMany(p => p.ContaCorrenteMateriais)
                .HasForeignKey(d => d.FuncionariosId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKContaCorre190438");

            entity.HasOne(d => d.Materiais).WithMany(p => p.ContaCorrenteMateriais)
                .HasForeignKey(d => d.MateriaisId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKContaCorre844530");

            entity.HasOne(d => d.PedidosMaterial).WithMany(p => p.ContaCorrenteMateriais)
                .HasForeignKey(d => d.PedidosMaterialId)
                .HasConstraintName("FKContaCorre76455");

            entity.HasOne(d => d.Utentes).WithMany(p => p.ContaCorrenteMateriais)
                .HasForeignKey(d => d.UtentesId)
                .HasConstraintName("FKContaCorre827649");
        });

        modelBuilder.Entity<ContaCorrenteMedicamento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ContaCor__3214EC07EBE3CF8D");

            entity.Property(e => e.Fatura)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Observacoes).IsUnicode(false);

            entity.HasOne(d => d.Funcionarios).WithMany(p => p.ContaCorrenteMedicamentos)
                .HasForeignKey(d => d.FuncionariosId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKContaCorre491345");

            entity.HasOne(d => d.Medicamentos).WithMany(p => p.ContaCorrenteMedicamentos)
                .HasForeignKey(d => d.MedicamentosId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKContaCorre835614");

            entity.HasOne(d => d.PedidosMedicamento).WithMany(p => p.ContaCorrenteMedicamentos)
                .HasForeignKey(d => d.PedidosMedicamentoId)
                .HasConstraintName("FKContaCorre86801");

            entity.HasOne(d => d.Utentes).WithMany(p => p.ContaCorrenteMedicamentos)
                .HasForeignKey(d => d.UtentesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKContaCorre854133");
        });

        modelBuilder.Entity<ContactosFornecedore>(entity =>
        {
            entity.HasKey(e => new { e.FornecedoresId, e.TipoContactoId }).HasName("PK__Contacto__A0BDE63FA6CA1885");

            entity.Property(e => e.Valor)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Fornecedores).WithMany(p => p.ContactosFornecedores)
                .HasForeignKey(d => d.FornecedoresId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKContactosF290738");

            entity.HasOne(d => d.TipoContacto).WithMany(p => p.ContactosFornecedores)
                .HasForeignKey(d => d.TipoContactoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKContactosF621134");
        });

        modelBuilder.Entity<ContactosFuncionario>(entity =>
        {
            entity.HasKey(e => new { e.FuncionariosId, e.TipoContactoId }).HasName("PK__Contacto__CD0B9060B302BA76");

            entity.Property(e => e.Valor)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Funcionarios).WithMany(p => p.ContactosFuncionarios)
                .HasForeignKey(d => d.FuncionariosId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKContactosF148783");

            entity.HasOne(d => d.TipoContacto).WithMany(p => p.ContactosFuncionarios)
                .HasForeignKey(d => d.TipoContactoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKContactosF548938");
        });

        modelBuilder.Entity<ContactosResponsavei>(entity =>
        {
            entity.HasKey(e => new { e.ResponsaveisId, e.TipoContactoId }).HasName("PK__Contacto__EB765284780A1C5D");

            entity.Property(e => e.Valor)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Responsaveis).WithMany(p => p.ContactosResponsaveis)
                .HasForeignKey(d => d.ResponsaveisId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKContactosR443200");

            entity.HasOne(d => d.TipoContacto).WithMany(p => p.ContactosResponsaveis)
                .HasForeignKey(d => d.TipoContactoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKContactosR909867");
        });

        modelBuilder.Entity<Equipamento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Equipame__3214EC0759F2BFA9");

            entity.Property(e => e.Descricao)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Quartos).WithMany(p => p.Equipamentos)
                .HasForeignKey(d => d.QuartosId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKEquipament648336");

            entity.HasOne(d => d.TiposEquipamento).WithMany(p => p.Equipamentos)
                .HasForeignKey(d => d.TiposEquipamentoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKEquipament615137");
        });

        modelBuilder.Entity<FeriasFuncionario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FeriasFu__3214EC07E7154705");

            entity.ToTable("FeriasFuncionario");

            entity.HasOne(d => d.Funcionarios).WithMany(p => p.FeriasFuncionarioFuncionarios)
                .HasForeignKey(d => d.FuncionariosId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKFeriasFunc710968");

            entity.HasOne(d => d.FuncionariosIdValidaNavigation).WithMany(p => p.FeriasFuncionarioFuncionariosIdValidaNavigations)
                .HasForeignKey(d => d.FuncionariosIdValida)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKFeriasFunc540384");
        });

        modelBuilder.Entity<Fornecedore>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Forneced__3214EC07AD829535");

            entity.Property(e => e.Nome)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Funcionario>(entity =>
        {
            entity.HasKey(e => e.FuncionarioId).HasName("PK__Funciona__3214EC07FB28FF8E");

            entity.Property(e => e.FuncionarioId).HasColumnName("FuncionarioID");
            entity.Property(e => e.Nome)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.TiposFuncionario).WithMany(p => p.Funcionarios)
                .HasForeignKey(d => d.TiposFuncionarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKFuncionari980374");
        });

        modelBuilder.Entity<Horario>(entity =>
        {
            entity.HasKey(e => new { e.FuncionariosId, e.TurnosId, e.Dia }).HasName("PK__Horarios__747460E3CC556000");

            entity.HasOne(d => d.Funcionarios).WithMany(p => p.Horarios)
                .HasForeignKey(d => d.FuncionariosId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKHorarios259703");

            entity.HasOne(d => d.Turnos).WithMany(p => p.Horarios)
                .HasForeignKey(d => d.TurnosId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKHorarios110747");
        });

        modelBuilder.Entity<Hospitai>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Hospitai__3214EC07E0190996");

            entity.Property(e => e.Ativo).HasDefaultValue(true);
            entity.Property(e => e.Morada)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Limpeza>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Limpezas__3214EC07B616BB9E");

            entity.HasOne(d => d.Funcionarios).WithMany(p => p.Limpezas)
                .HasForeignKey(d => d.FuncionariosId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKLimpezas2");

            entity.HasOne(d => d.Quartos).WithMany(p => p.Limpezas)
                .HasForeignKey(d => d.QuartosId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKLimpezas1");
        });

        modelBuilder.Entity<Materiai>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Materiai__3214EC07E5E0771D");

            entity.Property(e => e.Ativo).HasDefaultValue(true);
            entity.Property(e => e.Nome)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.TiposMaterial).WithMany(p => p.Materiais)
                .HasForeignKey(d => d.TiposMaterialId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKMateriais592894");
        });

        modelBuilder.Entity<MateriaisPlano>(entity =>
        {
            entity.HasKey(e => new { e.PlanosId, e.MateriaisId }).HasName("PK__Materiai__FE1CCEB0B0061D89");

            entity.ToTable("MateriaisPlano");

            entity.Property(e => e.Instrucoes)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.QuantidadePintervalo).HasColumnName("QuantidadePIntervalo");

            entity.HasOne(d => d.Materiais).WithMany(p => p.MateriaisPlanos)
                .HasForeignKey(d => d.MateriaisId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKMateriaisP622188");

            entity.HasOne(d => d.Planos).WithMany(p => p.MateriaisPlanos)
                .HasForeignKey(d => d.PlanosId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKMateriaisP28150");
        });

        modelBuilder.Entity<Medicamento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Medicame__3214EC0767D276AC");

            entity.Property(e => e.Ativo).HasDefaultValue(true);
            entity.Property(e => e.Descricao).IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasMany(d => d.Fornecedores).WithMany(p => p.Medicamentos)
                .UsingEntity<Dictionary<string, object>>(
                    "FornecedoresMedicamento",
                    r => r.HasOne<Fornecedore>().WithMany()
                        .HasForeignKey("FornecedoresId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FKFornecedor780678"),
                    l => l.HasOne<Medicamento>().WithMany()
                        .HasForeignKey("MedicamentosId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FKFornecedor377087"),
                    j =>
                    {
                        j.HasKey("MedicamentosId", "FornecedoresId").HasName("PK__Forneced__3093B33D1AA31F18");
                        j.ToTable("FornecedoresMedicamento");
                    });
        });

        modelBuilder.Entity<MedicamentosPrescricao>(entity =>
        {
            entity.HasKey(e => new { e.PrescricoesId, e.MedicamentosId }).HasName("PK__Medicame__318330E2D0EA1484");

            entity.ToTable("MedicamentosPrescricao");

            entity.Property(e => e.Instrucoes).IsUnicode(false);
            entity.Property(e => e.QuantidadePintervalo).HasColumnName("QuantidadePIntervalo");

            entity.HasOne(d => d.Medicamentos).WithMany(p => p.MedicamentosPrescricaos)
                .HasForeignKey(d => d.MedicamentosId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKMedicament170255");

            entity.HasOne(d => d.Prescricoes).WithMany(p => p.MedicamentosPrescricaos)
                .HasForeignKey(d => d.PrescricoesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKMedicament620837");
        });

        modelBuilder.Entity<Mensalidade>(entity =>
        {
            entity.HasKey(e => new { e.Mes, e.UtentesId }).HasName("PK__Mensalid__938B5DFE467A373C");

            entity.HasOne(d => d.TiposPagamento).WithMany(p => p.Mensalidades)
                .HasForeignKey(d => d.TiposPagamentoId)
                .HasConstraintName("FKMensalidad445929");

            entity.HasOne(d => d.Utentes).WithMany(p => p.Mensalidades)
                .HasForeignKey(d => d.UtentesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKMensalidad753041");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Menu__3214EC07995E8FAB");

            entity.ToTable("Menu");

            entity.HasOne(d => d.Pratos).WithMany(p => p.Menus)
                .HasForeignKey(d => d.PratosId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKMenu702490");

            entity.HasOne(d => d.Sobremesas).WithMany(p => p.Menus)
                .HasForeignKey(d => d.SobremesasId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKMenu502938");

            entity.HasOne(d => d.Sopas).WithMany(p => p.Menus)
                .HasForeignKey(d => d.SopasId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKMenu866710");
        });

        modelBuilder.Entity<Notificaco>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC07994B57F1");

            entity.Property(e => e.Mensagem).IsUnicode(false);

            entity.HasOne(d => d.TiposFuncionario).WithMany(p => p.Notificacos)
                .HasForeignKey(d => d.TiposFuncionarioId)
                .HasConstraintName("FKNotificacoes1");
        });

        modelBuilder.Entity<NotificacoesFuncionario>(entity =>
        {
            entity.HasKey(e => new { e.NotificacoesId, e.FuncionariosId }).HasName("PK__Notifica__94A4323A349D3187");

            entity.HasOne(d => d.Funcionarios).WithMany(p => p.NotificacoesFuncionarios)
                .HasForeignKey(d => d.FuncionariosId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKNotificacoesFuncionarios2");

            entity.HasOne(d => d.Notificacoes).WithMany(p => p.NotificacoesFuncionarios)
                .HasForeignKey(d => d.NotificacoesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKNotificacoesFuncionarios1");
        });

        modelBuilder.Entity<NotificacoesResponsavel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC0701B3E952");

            entity.ToTable("NotificacoesResponsavel");

            entity.Property(e => e.Mensagem).IsUnicode(false);

            entity.HasOne(d => d.Responsaveis).WithMany(p => p.NotificacoesResponsavels)
                .HasForeignKey(d => d.ResponsaveisId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKNotificacoesResponsavel1");
        });

        modelBuilder.Entity<PedidosMaterial>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PedidosM__3214EC07A2773A79");

            entity.ToTable("PedidosMaterial");

            entity.HasOne(d => d.Funcionarios).WithMany(p => p.PedidosMaterials)
                .HasForeignKey(d => d.FuncionariosId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKPedidosMat795229");

            entity.HasOne(d => d.Materiais).WithMany(p => p.PedidosMaterials)
                .HasForeignKey(d => d.MateriaisId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKPedidosMat449322");
        });

        modelBuilder.Entity<PedidosMedicamento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PedidosM__3214EC0750B34127");

            entity.ToTable("PedidosMedicamento");

            entity.HasOne(d => d.Funcionarios).WithMany(p => p.PedidosMedicamentos)
                .HasForeignKey(d => d.FuncionariosId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKPedidosMed822297");

            entity.HasOne(d => d.Medicamentos).WithMany(p => p.PedidosMedicamentos)
                .HasForeignKey(d => d.MedicamentosId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKPedidosMed495337");
        });

        modelBuilder.Entity<Plano>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Planos__3214EC07F36C8B2C");

            entity.Property(e => e.Observacoes).IsUnicode(false);

            entity.HasOne(d => d.Utentes).WithMany(p => p.Planos)
                .HasForeignKey(d => d.UtentesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKPlanos66493");
        });

        modelBuilder.Entity<Prato>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Pratos__3214EC0724F8B84A");

            entity.Property(e => e.Ativo).HasDefaultValue(true);
            entity.Property(e => e.Descricao).IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Prescrico>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Prescric__3214EC079C4A6830");

            entity.Property(e => e.Observacoes).IsUnicode(false);

            entity.HasOne(d => d.Utentes).WithMany(p => p.Prescricos)
                .HasForeignKey(d => d.UtentesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKPrescricoe661020");
        });

        modelBuilder.Entity<Quarto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Quartos__3214EC076D79477F");

            entity.HasOne(d => d.TiposQuarto).WithMany(p => p.Quartos)
                .HasForeignKey(d => d.TiposQuartoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKQuartos354658");
        });

        modelBuilder.Entity<Responsavei>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Responsa__3214EC0750C2B000");

            entity.Property(e => e.Morada)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Utentes).WithMany(p => p.Responsaveis)
                .HasForeignKey(d => d.UtentesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKResponsave278988");
        });

        modelBuilder.Entity<Senha>(entity =>
        {
            entity.HasKey(e => new { e.FuncionariosId, e.MenuId }).HasName("PK__Senhas__0334956E2B8392A6");

            entity.HasOne(d => d.Funcionarios).WithMany(p => p.Senhas)
                .HasForeignKey(d => d.FuncionariosId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKSenhas625312");

            entity.HasOne(d => d.Menu).WithMany(p => p.Senhas)
                .HasForeignKey(d => d.MenuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKSenhas417331");
        });

        modelBuilder.Entity<Sesso>(entity =>
        {
            entity.HasKey(e => e.SessaoId).HasName("PK__Sessoes__6E6330F9649D5189");

            entity.Property(e => e.SessaoId).HasColumnName("SessaoID");
            entity.Property(e => e.Observacoes)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Funcionarios).WithMany(p => p.Sessos)
                .HasForeignKey(d => d.FuncionariosId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKSessoes");

            entity.HasOne(d => d.Utentes).WithMany(p => p.Sessos)
                .HasForeignKey(d => d.UtentesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKSessoes509961");
        });

        modelBuilder.Entity<Sobremesa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Sobremes__3214EC0746993585");

            entity.Property(e => e.Ativo).HasDefaultValue(true);
            entity.Property(e => e.Descricao).IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Sopa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Sopas__3214EC071AFF6639");

            entity.Property(e => e.Ativo).HasDefaultValue(true);
            entity.Property(e => e.Descricao).IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TipoAvaliacao>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TipoAval__3214EC07305AAA0B");

            entity.ToTable("TipoAvaliacao");

            entity.Property(e => e.Descricao)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TipoContacto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TipoCont__3214EC075B8D172E");

            entity.ToTable("TipoContacto");

            entity.Property(e => e.Descricao)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TipoSessao>(entity =>
        {
            entity.HasKey(e => e.SessaoId).HasName("PK__TipoSess__6E6330F9697D63E7");

            entity.ToTable("TipoSessao");

            entity.Property(e => e.SessaoId)
                .ValueGeneratedNever()
                .HasColumnName("SessaoID");
            entity.Property(e => e.DescritcaoTipoSessao)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descritcaoTipoSessao");

            entity.HasOne(d => d.Sessao).WithOne(p => p.TipoSessao)
                .HasForeignKey<TipoSessao>(d => d.SessaoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKTipoSessao184475");
        });

        modelBuilder.Entity<TiposAdmissao>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TiposAdm__3214EC07489176E7");

            entity.ToTable("TiposAdmissao");

            entity.Property(e => e.Descricao)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TiposAlergium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TiposAle__3214EC075E4ABD8E");

            entity.Property(e => e.Descricao)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TiposEquipamento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TiposEqu__3214EC071CC0EE6E");

            entity.ToTable("TiposEquipamento");

            entity.Property(e => e.Descricao)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TiposFuncionario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TiposFun__3214EC07C11AB071");

            entity.ToTable("TiposFuncionario");

            entity.Property(e => e.Descricao)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TiposMaterial>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TiposMat__3214EC07A3E39202");

            entity.ToTable("TiposMaterial");

            entity.Property(e => e.Descricao)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TiposPagamento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TiposPag__3214EC0712FD89AA");

            entity.ToTable("TiposPagamento");

            entity.Property(e => e.Descricao)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TiposQuarto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TiposQua__3214EC075DE1A550");

            entity.ToTable("TiposQuarto");

            entity.Property(e => e.Descricao)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Turno>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Turnos__3214EC070A259CF8");
        });

        modelBuilder.Entity<UserFuncionario>(entity =>
        {
            entity.HasKey(e => e.FuncionarioId).HasName("PK__UserFunc__297ECD4AD2053F8A");

            entity.ToTable("UserFuncionario");

            entity.Property(e => e.FuncionarioId).HasColumnName("FuncionarioID");
            entity.Property(e => e.Id).HasMaxLength(128);
            entity.Property(e => e.Passe).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(256);
        });

        modelBuilder.Entity<UserResponsavel>(entity =>
        {
            entity.HasKey(e => e.ResponsaveisId).HasName("PK__UserResp__39D0BAA99957710B");

            entity.ToTable("UserResponsavel");

            entity.Property(e => e.ResponsaveisId).ValueGeneratedNever();
            entity.Property(e => e.Passe)
                .HasMaxLength(60)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.User)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Responsaveis).WithOne(p => p.UserResponsavel)
                .HasForeignKey<UserResponsavel>(d => d.ResponsaveisId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKUserRespon855353");
        });

        modelBuilder.Entity<Utente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Utentes__3214EC072703D681");

            entity.Property(e => e.AntecedentesPessoais).IsUnicode(false);
            entity.Property(e => e.DataAdmissao).HasColumnType("datetime");
            entity.Property(e => e.DiagnosticoAdmissao).IsUnicode(false);
            entity.Property(e => e.ExameObjetivo).IsUnicode(false);
            entity.Property(e => e.MotivoAdmissao).IsUnicode(false);
            entity.Property(e => e.Nif).HasColumnName("NIF");
            entity.Property(e => e.Nome)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.NotaAdmissao).IsUnicode(false);
            entity.Property(e => e.Observacoes).IsUnicode(false);
            entity.Property(e => e.Sns).HasColumnName("SNS");

            entity.HasOne(d => d.TiposAdmissao).WithMany(p => p.Utentes)
                .HasForeignKey(d => d.TiposAdmissaoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKUtentes747797");

            entity.HasMany(d => d.TiposAlergia).WithMany(p => p.Utentes)
                .UsingEntity<Dictionary<string, object>>(
                    "UtentesAlergia",
                    r => r.HasOne<TiposAlergium>().WithMany()
                        .HasForeignKey("TiposAlergiaId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FKUtentesAle119455"),
                    l => l.HasOne<Utente>().WithMany()
                        .HasForeignKey("UtentesId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FKUtentesAle365322"),
                    j =>
                    {
                        j.HasKey("UtentesId", "TiposAlergiaId").HasName("PK__UtentesA__45C1D1F837A7584F");
                        j.ToTable("UtentesAlergias");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
