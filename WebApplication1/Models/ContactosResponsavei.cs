using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class ContactosResponsavei
{
    public int ResponsaveisId { get; set; }

    public int TipoContactoId { get; set; }

    public string Valor { get; set; } = null!;

    public virtual Responsavei Responsaveis { get; set; } = null!;

    public virtual TipoContacto TipoContacto { get; set; } = null!;
}
