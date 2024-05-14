using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Funcionario
{
    public int FuncionarioId { get; set; }

    public string Nome { get; set; } = null!;

    public int TiposFuncionarioId { get; set; }

    public bool Historico { get; set; }

    public virtual ICollection<Altum> Alta { get; set; } = new List<Altum>();

    public virtual ICollection<Avaliaco> Avaliacos { get; set; } = new List<Avaliaco>();

    public virtual ICollection<Consulta> Consulta { get; set; } = new List<Consulta>();

    public virtual ICollection<ContaCorrenteMateriai> ContaCorrenteMateriais { get; set; } = new List<ContaCorrenteMateriai>();

    public virtual ICollection<ContaCorrenteMedicamento> ContaCorrenteMedicamentos { get; set; } = new List<ContaCorrenteMedicamento>();

    public virtual ICollection<ContactosFuncionario> ContactosFuncionarios { get; set; } = new List<ContactosFuncionario>();

    public virtual ICollection<FeriasFuncionario> FeriasFuncionarioFuncionarios { get; set; } = new List<FeriasFuncionario>();

    public virtual ICollection<FeriasFuncionario> FeriasFuncionarioFuncionariosIdValidaNavigations { get; set; } = new List<FeriasFuncionario>();

    public virtual ICollection<Horario> Horarios { get; set; } = new List<Horario>();

    public virtual ICollection<Limpeza> Limpezas { get; set; } = new List<Limpeza>();

    public virtual ICollection<NotificacoesFuncionario> NotificacoesFuncionarios { get; set; } = new List<NotificacoesFuncionario>();

    public virtual ICollection<PedidosMaterial> PedidosMaterials { get; set; } = new List<PedidosMaterial>();

    public virtual ICollection<PedidosMedicamento> PedidosMedicamentos { get; set; } = new List<PedidosMedicamento>();

    public virtual ICollection<Senha> Senhas { get; set; } = new List<Senha>();

    public virtual ICollection<Sesso> Sessos { get; set; } = new List<Sesso>();

    public virtual TiposFuncionario TiposFuncionario { get; set; } = null!;
}
