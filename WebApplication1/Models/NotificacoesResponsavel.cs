using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class NotificacoesResponsavel
{
    public int Id { get; set; }

    public string Mensagem { get; set; } = null!;

    public int Estado { get; set; }

    public int ResponsaveisId { get; set; }

    public virtual Responsavei Responsaveis { get; set; } = null!;
}
