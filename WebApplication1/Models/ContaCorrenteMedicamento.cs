using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class ContaCorrenteMedicamento
{
    public int Id { get; set; }

    public string? Fatura { get; set; }

    public int MedicamentosId { get; set; }

    public int? PedidosMedicamentoId { get; set; }

    public int FuncionariosId { get; set; }

    public int UtentesId { get; set; }

    public DateOnly Data { get; set; }

    public bool Tipo { get; set; }

    public int QuantidadeMovimento { get; set; }

    public string? Observacoes { get; set; }

    public virtual Funcionario Funcionarios { get; set; } = null!;

    public virtual Medicamento Medicamentos { get; set; } = null!;

    public virtual PedidosMedicamento? PedidosMedicamento { get; set; }

    public virtual Utente Utentes { get; set; } = null!;
}
