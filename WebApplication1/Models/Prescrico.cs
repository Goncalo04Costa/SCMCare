using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Prescrico
{
    public int Id { get; set; }

    public int UtentesId { get; set; }

    public DateOnly DataInicio { get; set; }

    public DateOnly? DataFim { get; set; }

    public string Observacoes { get; set; } = null!;

    public virtual ICollection<MedicamentosPrescricao> MedicamentosPrescricaos { get; set; } = new List<MedicamentosPrescricao>();

    public virtual Utente Utentes { get; set; } = null!;
}
