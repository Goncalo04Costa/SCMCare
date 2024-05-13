using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class ContactosFuncionario
{
    public int FuncionariosId { get; set; }

    public int TipoContactoId { get; set; }

    public string Valor { get; set; } = null!;

    public virtual Funcionario Funcionarios { get; set; } = null!;

    public virtual TipoContacto TipoContacto { get; set; } = null!;
}
