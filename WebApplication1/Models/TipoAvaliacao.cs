using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class TipoAvaliacao
{
    public int Id { get; set; }

    public string Descricao { get; set; } = null!;

    public virtual ICollection<Avaliaco> Avaliacos { get; set; } = new List<Avaliaco>();
}
