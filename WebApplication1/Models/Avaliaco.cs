using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Avaliaco
{
    public int UtentesId { get; set; }

    public int FuncionariosId { get; set; }

    public string? Analise { get; set; }

    public DateTime Data { get; set; }

    public int TipoAvaliacaoId { get; set; }

    public string AuscultacaoPolmunar { get; set; } = null!;

    public string AucultacaoCardiaca { get; set; } = null!;

    public virtual Funcionario Funcionarios { get; set; } = null!;

    public virtual TipoAvaliacao TipoAvaliacao { get; set; } = null!;

    public virtual Utente Utentes { get; set; } = null!;
}
