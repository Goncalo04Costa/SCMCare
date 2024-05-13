using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Plano
{
    public int Id { get; set; }

    public int UtentesId { get; set; }

    public DateOnly DataInicio { get; set; }

    public DateOnly? DataFim { get; set; }

    public string? Observacoes { get; set; }

    public virtual ICollection<MateriaisPlano> MateriaisPlanos { get; set; } = new List<MateriaisPlano>();

    public virtual Utente Utentes { get; set; } = null!;
}
