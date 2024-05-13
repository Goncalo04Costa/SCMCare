using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class UserResponsavel
{
    public int ResponsaveisId { get; set; }

    public string User { get; set; } = null!;

    public string Passe { get; set; } = null!;

    public virtual Responsavei Responsaveis { get; set; } = null!;
}
