using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class TiposQuarto
{
    public int Id { get; set; }

    public string Descricao { get; set; } = null!;

    public virtual ICollection<Quarto> Quartos { get; set; } = new List<Quarto>();
}
