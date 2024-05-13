using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Quarto
{
    public int Id { get; set; }

    public int Numero { get; set; }

    public int TiposQuartoId { get; set; }

    public virtual ICollection<Cama> Camas { get; set; } = new List<Cama>();

    public virtual ICollection<Equipamento> Equipamentos { get; set; } = new List<Equipamento>();

    public virtual ICollection<Limpeza> Limpezas { get; set; } = new List<Limpeza>();

    public virtual TiposQuarto TiposQuarto { get; set; } = null!;
}
