using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Menu
{
    public int Id { get; set; }

    public DateOnly Dia { get; set; }

    public bool Horario { get; set; }

    public bool Tipo { get; set; }

    public int SopasId { get; set; }

    public int PratosId { get; set; }

    public int SobremesasId { get; set; }

    public virtual Prato Pratos { get; set; } = null!;

    public virtual ICollection<Senha> Senhas { get; set; } = new List<Senha>();

    public virtual Sobremesa Sobremesas { get; set; } = null!;

    public virtual Sopa Sopas { get; set; } = null!;
}
