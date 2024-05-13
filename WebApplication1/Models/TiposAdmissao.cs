using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class TiposAdmissao
{
    public int Id { get; set; }

    public string Descricao { get; set; } = null!;

    public virtual ICollection<Utente> Utentes { get; set; } = new List<Utente>();
}
