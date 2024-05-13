using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class TiposFuncionario
{
    public int Id { get; set; }

    public string Descricao { get; set; } = null!;

    public virtual ICollection<Funcionario> Funcionarios { get; set; } = new List<Funcionario>();

    public virtual ICollection<Notificaco> Notificacos { get; set; } = new List<Notificaco>();
}
