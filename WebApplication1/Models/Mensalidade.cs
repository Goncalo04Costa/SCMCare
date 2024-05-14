using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Mensalidade
{
    public DateOnly Mes { get; set; }

    public DateOnly? DataPagamento { get; set; }

    public int UtentesId { get; set; }

    public int? TiposPagamentoId { get; set; }

    public int Estado { get; set; }

    public virtual TiposPagamento? TiposPagamento { get; set; }

    public virtual Utente Utentes { get; set; } = null!;
}
