using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class PedidosMaterial
{
    public int Id { get; set; }

    public int MateriaisId { get; set; }

    public int FuncionariosId { get; set; }

    public int QuantidadeTotal { get; set; }

    public DateOnly DataPedido { get; set; }

    public int Estado { get; set; }

    public DateOnly? DataConclusao { get; set; }

    public virtual ICollection<ContaCorrenteMateriai> ContaCorrenteMateriais { get; set; } = new List<ContaCorrenteMateriai>();

    public virtual Funcionario Funcionarios { get; set; } = null!;

    public virtual Materiai Materiais { get; set; } = null!;
}
