using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Horario
{
    public int FuncionariosId { get; set; }

    public int TurnosId { get; set; }

    public DateOnly Dia { get; set; }

    public virtual Funcionario Funcionarios { get; set; } = null!;

    public virtual Turno Turnos { get; set; } = null!;
}
