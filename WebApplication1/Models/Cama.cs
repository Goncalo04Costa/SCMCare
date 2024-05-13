using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Cama
{
    public int UtentesId { get; set; }

    public int QuartosId { get; set; }

    public int Id { get; set; }

    public virtual Quarto Quartos { get; set; } = null!;

    public virtual Utente Utentes { get; set; } = null!;
}
