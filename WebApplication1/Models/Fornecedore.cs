using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Fornecedore
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public virtual ICollection<ContactosFornecedore> ContactosFornecedores { get; set; } = new List<ContactosFornecedore>();

    public virtual ICollection<Medicamento> Medicamentos { get; set; } = new List<Medicamento>();
}
