using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class FeriasFuncionario
{
    public int Id { get; set; }

    public int FuncionariosId { get; set; }

    public int FuncionariosIdValida { get; set; }

    public DateOnly Dia { get; set; }

    public int Estado { get; set; }

    public virtual Funcionario Funcionarios { get; set; } = null!;

    public virtual Funcionario FuncionariosIdValidaNavigation { get; set; } = null!;
}
