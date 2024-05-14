using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Limpeza
{
    public int Id { get; set; }

    public DateOnly Data { get; set; }

    public int QuartosId { get; set; }

    public int FuncionariosId { get; set; }

    public virtual Funcionario Funcionarios { get; set; } = null!;

    public virtual Quarto Quartos { get; set; } = null!;
}
