using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class TipoContacto
{
    public int Id { get; set; }

    public string Descricao { get; set; } = null!;

    public virtual ICollection<ContactosFornecedore> ContactosFornecedores { get; set; } = new List<ContactosFornecedore>();

    public virtual ICollection<ContactosFuncionario> ContactosFuncionarios { get; set; } = new List<ContactosFuncionario>();

    public virtual ICollection<ContactosResponsavei> ContactosResponsaveis { get; set; } = new List<ContactosResponsavei>();
}
