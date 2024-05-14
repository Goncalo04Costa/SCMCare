using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class PedidosMedicamento
{
    public int Id { get; set; }

    public int MedicamentosId { get; set; }

    public int FuncionariosId { get; set; }

    public int Quantidade { get; set; }

    public DateOnly DataPedido { get; set; }

    public int Estado { get; set; }

    public DateOnly? DataConclusao { get; set; }

    public virtual ICollection<ContaCorrenteMedicamento> ContaCorrenteMedicamentos { get; set; } = new List<ContaCorrenteMedicamento>();

    public virtual Funcionario Funcionarios { get; set; } = null!;

    public virtual Medicamento Medicamentos { get; set; } = null!;
}
