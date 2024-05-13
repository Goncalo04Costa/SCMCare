using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class TiposPagamento
{
    public int Id { get; set; }

    public string Descricao { get; set; } = null!;

    public virtual ICollection<Mensalidade> Mensalidades { get; set; } = new List<Mensalidade>();
}
