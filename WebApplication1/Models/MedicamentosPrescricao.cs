using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class MedicamentosPrescricao
{
    public int PrescricoesId { get; set; }

    public int MedicamentosId { get; set; }

    public int QuantidadePintervalo { get; set; }

    public int IntervaloHoras { get; set; }

    public string Instrucoes { get; set; } = null!;

    public virtual Medicamento Medicamentos { get; set; } = null!;

    public virtual Prescrico Prescricoes { get; set; } = null!;
}
