using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class NotificacoesFuncionario
{
    public int NotificacoesId { get; set; }

    public int FuncionariosId { get; set; }

    public int Estado { get; set; }

    public virtual Funcionario Funcionarios { get; set; } = null!;

    public virtual Notificaco Notificacoes { get; set; } = null!;
}
