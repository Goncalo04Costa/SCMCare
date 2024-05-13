using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class MateriaisPlano
{
    public int PlanosId { get; set; }

    public int MateriaisId { get; set; }

    public int QuantidadePintervalo { get; set; }

    public int IntervaloHoras { get; set; }

    public string Instrucoes { get; set; } = null!;

    public virtual Materiai Materiais { get; set; } = null!;

    public virtual Plano Planos { get; set; } = null!;
}
