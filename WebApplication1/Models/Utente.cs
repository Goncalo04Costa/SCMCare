using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Utente
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public int Nif { get; set; }

    public int Sns { get; set; }

    public DateTime DataAdmissao { get; set; }

    public DateOnly DataNascimento { get; set; }

    public bool Historico { get; set; }

    public bool Tipo { get; set; }

    public int TiposAdmissaoId { get; set; }

    public string? MotivoAdmissao { get; set; }

    public string? DiagnosticoAdmissao { get; set; }

    public string? Observacoes { get; set; }

    public string? NotaAdmissao { get; set; }

    public string? AntecedentesPessoais { get; set; }

    public string? ExameObjetivo { get; set; }

    public double Mensalidade { get; set; }

    public double Cofinanciamento { get; set; }

    public virtual Altum? Altum { get; set; }

    public virtual ICollection<Avaliaco> Avaliacos { get; set; } = new List<Avaliaco>();

    public virtual Cama? Cama { get; set; }

    public virtual ICollection<Consulta> Consulta { get; set; } = new List<Consulta>();

    public virtual ICollection<ContaCorrenteMateriai> ContaCorrenteMateriais { get; set; } = new List<ContaCorrenteMateriai>();

    public virtual ICollection<ContaCorrenteMedicamento> ContaCorrenteMedicamentos { get; set; } = new List<ContaCorrenteMedicamento>();

    public virtual ICollection<Mensalidade> Mensalidades { get; set; } = new List<Mensalidade>();

    public virtual ICollection<Plano> Planos { get; set; } = new List<Plano>();

    public virtual ICollection<Prescrico> Prescricos { get; set; } = new List<Prescrico>();

    public virtual ICollection<Responsavei> Responsaveis { get; set; } = new List<Responsavei>();

    public virtual ICollection<Sesso> Sessos { get; set; } = new List<Sesso>();

    public virtual TiposAdmissao TiposAdmissao { get; set; } = null!;

    public virtual ICollection<TiposAlergium> TiposAlergia { get; set; } = new List<TiposAlergium>();
}
