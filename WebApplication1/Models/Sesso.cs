using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Sesso
{
    public int FuncionariosId { get; set; }

    public int UtentesId { get; set; }

    public int SessaoId { get; set; }

    public DateOnly Dia { get; set; }

    public string? Observacoes { get; set; }

    public virtual Funcionario Funcionarios { get; set; } = null!;

    public virtual TipoSessao? TipoSessao { get; set; }

    public virtual Utente Utentes { get; set; } = null!;
}
