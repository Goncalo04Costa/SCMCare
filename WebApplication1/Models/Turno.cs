using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Turno
{
    public int Id { get; set; }

    public TimeOnly HoraInicio { get; set; }

    public TimeOnly HoraFim { get; set; }

    public bool Ativo { get; set; }

    public virtual ICollection<Horario> Horarios { get; set; } = new List<Horario>();
}
