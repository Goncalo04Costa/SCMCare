using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Materiai
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public int? Limite { get; set; }

    public int TiposMaterialId { get; set; }

    public bool Ativo { get; set; }

    public virtual ICollection<ContaCorrenteMateriai> ContaCorrenteMateriais { get; set; } = new List<ContaCorrenteMateriai>();

    public virtual ICollection<MateriaisPlano> MateriaisPlanos { get; set; } = new List<MateriaisPlano>();

    public virtual ICollection<PedidosMaterial> PedidosMaterials { get; set; } = new List<PedidosMaterial>();

    public virtual TiposMaterial TiposMaterial { get; set; } = null!;
}
