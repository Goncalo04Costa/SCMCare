using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Consulta
{
    public int Id { get; set; }

    public DateOnly Data { get; set; }

    public string Descricao { get; set; } = null!;

    public int HospitaisId { get; set; }

    public int UtentesId { get; set; }

    public int FuncionariosId { get; set; }

    public int ResponsaveisId { get; set; }

    public virtual Funcionario Funcionarios { get; set; } = null!;

    public virtual Hospitai Hospitais { get; set; } = null!;

    public virtual Responsavei Responsaveis { get; set; } = null!;

    public virtual Utente Utentes { get; set; } = null!;
}
