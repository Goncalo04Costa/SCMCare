using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class TipoSessao
{
    public int SessaoId { get; set; }

    public string? DescritcaoTipoSessao { get; set; }

    public virtual Sesso Sessao { get; set; } = null!;
}
