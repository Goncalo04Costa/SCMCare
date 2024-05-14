using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Notificaco
{
    public int Id { get; set; }

    public string Mensagem { get; set; } = null!;

    public int? TiposFuncionarioId { get; set; }

    public virtual ICollection<NotificacoesFuncionario> NotificacoesFuncionarios { get; set; } = new List<NotificacoesFuncionario>();

    public virtual TiposFuncionario? TiposFuncionario { get; set; }
}
