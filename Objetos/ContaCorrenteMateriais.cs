/*
*	<copyright file="ContaCorrenteMateriais" company="IPCA">
*	</copyright>
* 	<author>Gonçalo Costa</author>
*	<contact>a26024@alunos.ipca.pt</contact>
*   <date>2024 24/03/2024 12:12:53</date>
*	<description></description>
**/

using System;
using System.Collections.Generic;
using System.Data;
using MetodosGlobais;

namespace ObjetosNegocio
{
    public class ContaCorrenteMateriais
    {
        #region Atributos

        public int Id { get; set; }
        public string Fatura { get; set; }
        public int MateriaisId { get; set; }
        public int? PedidosMaterialId { get; set; }
        public int FuncionariosId { get; set; }
        public int? UtentesId { get; set; }
        public DateTime Data { get; set; }
        public bool Tipo { get; set; }
        public int QuantidadeMovimento { get; set; }
        public string Observacoes { get; set; }

        #endregion

        #region Métodos

        #region Construtores
        public ContaCorrenteMateriais() { }

        /// <summary>
        /// Construtor para conta corrente de materiais.
        /// Recebe uma tabela com dados e de acordo com as colunas vai adicionar ao objeto.
        /// </summary>
        /// <param name="tabela">Tabela de dados.</param>
        public ContaCorrenteMateriais(DataRow tabela)
        {
            if (tabela.Table.Columns.Contains("Id"))
            {
                this.Id = tabela.Field<int>("Id");
            }
            if (tabela.Table.Columns.Contains("Fatura"))
            {
                this.Fatura = tabela.Field<string>("Fatura");
            }
            if (tabela.Table.Columns.Contains("MateriaisId"))
            {
                this.MateriaisId = tabela.Field<int>("MateriaisId");
            }
            if (tabela.Table.Columns.Contains("PedidosMaterialId"))
            {
                this.PedidosMaterialId = tabela.Field<int?>("PedidosMaterialId");
            }
            if (tabela.Table.Columns.Contains("FuncionariosId"))
            {
                this.FuncionariosId = tabela.Field<int>("FuncionariosId");
            }
            if (tabela.Table.Columns.Contains("UtentesId"))
            {
                this.UtentesId = tabela.Field<int?>("UtentesId");
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
        #endregion

        #region Outros Métodos

        /// <summary>
        /// Método para obter a lista de conta corrente de materiais de acordo com o filtro.
        /// </summary>
        /// <param name="filtros">Filtro de parâmetros.</param>
        /// <returns>Devolve a lista de conta corrente de materiais.</returns>
        public static List<ContaCorrenteMateriais> ObterLista(Dictionary<String, Object> filtros)
        {
            string sql;
            PreparaSQL(filtros, out sql);

            List<ContaCorrenteMateriais> lstCCM = Geral<ContaCorrenteMateriais>.ObterLista(sql);

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
            sql = @"SELECT Id, Fatura, MateriaisId, PedidosMaterialId, FuncionariosId, UtentesId, Data, Tipo, QuantidadeMovimento, Observacoes FROM ContaCorrenteMateriais WHERE 1=1 ";

            // Adicionar filtros à SQL e registar os parâmetros
            if (filtros != null)
            {
                // Adicione mais filtros conforme necessário
            }
        }

        public static int Inserir(ContaCorrenteMateriais ccm)
        {
            string sql;
            sql = "INSERT INTO ContaCorrenteMateriais (Fatura, MateriaisId, PedidosMaterialId, FuncionariosId, UtentesId, Data, Tipo, QuantidadeMovimento, Observacoes) VALUES ('" + ccm.Fatura + "', " + ccm.MateriaisId + ", " + (ccm.PedidosMaterialId != null ? ccm.PedidosMaterialId.ToString() : "NULL") + ", " + ccm.FuncionariosId + ", " + (ccm.UtentesId != null ? ccm.UtentesId.ToString() : "NULL") + ", '" + ccm.Data.ToString("yyyy-MM-dd") + "', " + (ccm.Tipo ? "1" : "0") + ", " + ccm.QuantidadeMovimento + ", '" + ccm.Observacoes + "')";

            return Geral.Manipular(sql);
        }


        public static int Remover(int id)
        {
            string sql;
            sql = "DELETE FROM ContaCorrenteMateriais WHERE Id = " + id;
            return Geral.Manipular(sql);
        }

        #endregion

        #endregion
    }
}
