using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Sobremesa
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public string? Descricao { get; set; }

    public bool Tipo { get; set; }

    public bool Ativo { get; set; }

    public virtual ICollection<Menu> Menus { get; set; } = new List<Menu>();
}
