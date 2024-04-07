/*
 *  <copyright file="ContaCorrenteMedicamentos" company="IPCA">
 *  </copyright>
 *  <author>Gonçalo Costa</author>
 *  <contact>a26024@alunos.ipca.pt</contact>
*   <date>2024 24/03/2024 12:13:03</date>
*	<description></description>
*/


using MetodosGlobais;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjetosNegocio
{
    public class ContaCorrenteMedicamentos2
    {
        #region Atributos
        public int Id { get; set; }
        public string Fatura { get; set; }
        public int MedicamentosId { get; set;}
        public int? PedidosMedicamentoId { get; set; }
        public int FuncionariosId { get; set; }
        public int UtentesId { get; set; }
        public DateTime Data {get; set;}
        public bool Tipo { get; set;}
        public int QuantidadeMovimento {  get; set; }
        public string Observacoes { get; set; }

        #endregion

        #region Métodos 

        #region Construtores 
        public ContaCorrenteMedicamentos2() { }
        /// Construtor para conta corrente de medicamentos.
        /// Recebe uma tabela com dados e de acordo com as colunas vai adicionar ao objeto.
        /// </summary>
        /// <param name="tabela">Tabela de dados.</param>
        /// 

        public ContaCorrenteMedicamentos2(DataRow tabela)
        {
            if (tabela.Table.Columns.Contains("Id"))
            {
                    this.Id = tabela.Field<int>("Id");
            }
            if (tabela.Table.Columns.Contains("Fatura"))
            {
                this.Fatura = tabela.Field<string>("Fatura");
            }
            if (tabela.Table.Columns.Contains("MedicamentosId"))
            {
                this.MedicamentosId = tabela.Field<int>("MedicamentosId");
            }
            if (tabela.Table.Columns.Contains("PedidosMedicamentoId"))
            {
                this.PedidosMedicamentoId = tabela.Field<int?>("PedidosMedicamentoId");
            }
            if (tabela.Table.Columns.Contains("FuncionariosId"))
            {
                this.FuncionariosId = tabela.Field<int>("FuncionariosId");
            }
            if (tabela.Table.Columns.Contains("UtentesId"))
            {
                this.UtentesId = tabela.Field<int>("UtentesId");
            }
            if (tabela.Table.Columns.Contains("Data"))
            {
                this.Data = tabela.Field<DateTime>("Data");
            }
            if (tabela.Table.Columns.Contains("Tipo"))
            {
                this.Tipo = tabela.Field<bool>("Tipo");
            }
            if (tabela.Table.Columns.Contains("QuantidadeMovimento"))
            {
                this.QuantidadeMovimento = tabela.Field<int>("QuantidadeMovimento");
            }
            if (tabela.Table.Columns.Contains("Observacoes"))
            {
                this.Observacoes = tabela.Field<string>("Observacoes");
            }
        }
    }
    #endregion

    #region Outros Métodos 

    /// <summary>
    /// Método para obter a lista de conta corrente de medicamentos de acordo com o filtro.
    /// </summary>
    /// <param name="filtros">Filtro de parâmetros.</param>
    /// <returns>Devolve a lista de conta corrente de medicamentos.</returns>
    public static List<ContaCorrenteMedicamentos> ObterLista(Dictionary<String, Object> filtros)
    {
        string sql;
        PreparaSQL(filtros, out sql);

        List<ContaCorrenteMedicamentos> lstCCM = Geral<ContaCorrenteMedicamentos>.ObterLista(sql);

        return lstCCM;
    }

    /// <summary>
    /// Método para preparar a query SQL com os filtros obtidos.
    /// </summary>
    /// <param name="filtros">Filtros a aplicar.</param>
    /// <param name="sql">Query SQL.</param>
    private static void PreparaSQL(Dictionary<String, Object> filtros, out string sql)
    {
        // Parâmetros a devolver no final
        List<object> parSQL = new List<object>();
        sql = @"SELECT Id, Fatura, MedicamentosId, PedidosMedicamentoId, FuncionariosId, UtentesId, Data, Tipo, QuantidadeMovimento, Observacoes FROM ContaCorrenteMedicamentos WHERE 1=1 ";

        // Adicionar filtros à SQL e registar os parâmetros
        if (filtros != null)
        {
            // Adicione mais filtros conforme necessário
        }
    }

    /// <summary>
    /// Método para inserir uma nova conta corrente de medicamentos.
    /// </summary>
    /// <param name="ccm">Objeto ContaCorrenteMedicamentos a ser inserido.</param>
    /// <returns>O número de linhas afetadas pela inserção.</returns>
    public static int Inserir(ContaCorrenteMedicamentos ccm)
    {
        string sql = "INSERT INTO ContaCorrenteMedicamentos (Fatura, MedicamentosId, PedidosMedicamentoId, FuncionariosId, UtentesId, Data, Tipo, QuantidadeMovimento, Observacoes) " +
                     "VALUES (@Fatura, @MedicamentosId, @PedidosMedicamentoId, @FuncionariosId, @UtentesId, @Data, @Tipo, @QuantidadeMovimento, @Observacoes)";

        Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "@Fatura", ccm.Fatura },
                { "@MedicamentosId", ccm.MedicamentosId },
                { "@PedidosMedicamentoId", ccm.PedidosMedicamentoId ?? (object)DBNull.Value },
                { "@FuncionariosId", ccm.FuncionariosId },
                { "@UtentesId", ccm.UtentesId },
                { "@Data", ccm.Data },
                { "@Tipo", ccm.Tipo },
                { "@QuantidadeMovimento", ccm.QuantidadeMovimento },
                { "@Observacoes", ccm.Observacoes }
            };

        return Geral.Manipular(sql, parametros);
    }

    /// <summary>
    /// Método para remover uma conta corrente de medicamentos pelo seu ID.
    /// </summary>
    /// <param name="id">ID da conta corrente de medicamentos a ser removida.</param>
    /// <returns>O número de linhas afetadas pela remoção.</returns>
    public static int Remover(int id)
    {
        string sql = "DELETE FROM ContaCorrenteMedicamentos WHERE Id = @Id";

        Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "@Id", id }
            };

        return Geral.Manipular(sql, parametros);
    }

    #endregion

    #endregion
}
}


}