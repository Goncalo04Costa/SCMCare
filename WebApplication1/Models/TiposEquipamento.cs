using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class TiposEquipamento
{
    public int Id { get; set; }

    public string Descricao { get; set; } = null!;

    public virtual ICollection<Equipamento> Equipamentos { get; set; } = new List<Equipamento>();
}
