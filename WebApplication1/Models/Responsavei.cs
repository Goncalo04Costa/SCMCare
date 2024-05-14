using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Responsavei
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public int UtentesId { get; set; }

    public string? Morada { get; set; }

    public virtual ICollection<Consulta> Consulta { get; set; } = new List<Consulta>();

    public virtual ICollection<ContactosResponsavei> ContactosResponsaveis { get; set; } = new List<ContactosResponsavei>();

    public virtual ICollection<NotificacoesResponsavel> NotificacoesResponsavels { get; set; } = new List<NotificacoesResponsavel>();

    public virtual UserResponsavel? UserResponsavel { get; set; }

    public virtual Utente Utentes { get; set; } = null!;
}
