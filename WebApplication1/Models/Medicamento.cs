using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Medicamento
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public string Descricao { get; set; } = null!;

    public int Limite { get; set; }

    public bool Ativo { get; set; }

    public virtual ICollection<ContaCorrenteMedicamento> ContaCorrenteMedicamentos { get; set; } = new List<ContaCorrenteMedicamento>();

    public virtual ICollection<MedicamentosPrescricao> MedicamentosPrescricaos { get; set; } = new List<MedicamentosPrescricao>();

    public virtual ICollection<PedidosMedicamento> PedidosMedicamentos { get; set; } = new List<PedidosMedicamento>();

    public virtual ICollection<Fornecedore> Fornecedores { get; set; } = new List<Fornecedore>();
}
