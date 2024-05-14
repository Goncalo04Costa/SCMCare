using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class TiposMaterial
{
    public int Id { get; set; }

    public string Descricao { get; set; } = null!;

    public virtual ICollection<Materiai> Materiais { get; set; } = new List<Materiai>();
}
