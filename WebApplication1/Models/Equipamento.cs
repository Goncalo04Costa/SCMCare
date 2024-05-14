using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Equipamento
{
    public int Id { get; set; }

    public string Descricao { get; set; } = null!;

    public bool Historico { get; set; }

    public int TiposEquipamentoId { get; set; }

    public int QuartosId { get; set; }

    public virtual ICollection<Avaria> Avaria { get; set; } = new List<Avaria>();

    public virtual Quarto Quartos { get; set; } = null!;

    public virtual TiposEquipamento TiposEquipamento { get; set; } = null!;
}
