using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class ContaCorrenteMateriai
{
    public int Id { get; set; }

    public string? Fatura { get; set; }

    public int MateriaisId { get; set; }

    public int? PedidosMaterialId { get; set; }

    public int FuncionariosId { get; set; }

    public int? UtentesId { get; set; }

    public DateOnly Data { get; set; }

    public bool Tipo { get; set; }

    public int QuantidadeMovimento { get; set; }

    public string? Observacoes { get; set; }

    public virtual Funcionario Funcionarios { get; set; } = null!;

    public virtual Materiai Materiais { get; set; } = null!;

    public virtual PedidosMaterial? PedidosMaterial { get; set; }

    public virtual Utente? Utentes { get; set; }
}
