using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Avaria
{
    public int Id { get; set; }

    public DateOnly Data { get; set; }

    public int EquipamentosId { get; set; }

    public string? Descricao { get; set; }

    public int Estado { get; set; }

    public virtual Equipamento Equipamentos { get; set; } = null!;
}
