using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class ContactosFornecedore
{
    public int FornecedoresId { get; set; }

    public int TipoContactoId { get; set; }

    public string Valor { get; set; } = null!;

    public virtual Fornecedore Fornecedores { get; set; } = null!;

    public virtual TipoContacto TipoContacto { get; set; } = null!;
}
