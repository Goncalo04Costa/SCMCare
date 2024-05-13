using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Altum
{
    public int UtentesId { get; set; }

    public int FuncionariosId { get; set; }

    public DateOnly Data { get; set; }

    public string? Motivo { get; set; }

    public string? Destino { get; set; }

    public virtual Funcionario Funcionarios { get; set; } = null!;

    public virtual Utente Utentes { get; set; } = null!;
}
