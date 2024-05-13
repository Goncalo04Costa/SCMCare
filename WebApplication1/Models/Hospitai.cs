using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Hospitai
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public string Morada { get; set; } = null!;

    public bool Ativo { get; set; }

    public virtual ICollection<Consulta> Consulta { get; set; } = new List<Consulta>();
}
