using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Senha
{
    public int FuncionariosId { get; set; }

    public int MenuId { get; set; }

    public int Estado { get; set; }

    public virtual Funcionario Funcionarios { get; set; } = null!;

    public virtual Menu Menu { get; set; } = null!;
}
