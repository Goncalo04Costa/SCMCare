/*
*	<copyright file="PedidosMaterial" company="IPCA">
*	</copyright>
* 	<author>Gonçalo Costa</author>
*	<contact>a26024@alunos.ipca.pt</contact>
*   <date>2024 24/03/2024 11:40:38</date>
*	<description></description>
**/

using System;
using System.Collections.Generic;
using System.Data;
using Geral;

namespace Objetos
{
    public class PedidosMaterial
    {
        #region Atributos

        public int Id { get; set; }
        public int MateriaisId { get; set; }
        public int FuncionariosId { get; set; }
        public int QuantidadeTotal { get; set; }
        public DateTime DataPedido { get; set; }
        public int Estado { get; set; }
        public DateTime? DataConclusao { get; set; }

        #endregion

        #region Métodos

        #region Construtores
        public PedidosMaterial() { }

        /// <summary>
        /// Construtor para pedidos de material.
        /// Recebe uma tabela com dados e de acordo com as colunas vai adicionar ao objeto.
        /// </summary>
        /// <param name="tabela">Tabela de dados.</param>
        public PedidosMaterial(DataRow tabela)
        {
            if (tabela.Table.Columns.Contains("Id"))
            {
                this.Id = tabela.Field<int>("Id");
            }
            if (tabela.Table.Columns.Contains("MateriaisId"))
            {
                this.MateriaisId = tabela.Field<int>("MateriaisId");
            }
            if (tabela.Table.Columns.Contains("FuncionariosId"))
            {
                this.FuncionariosId = tabela.Field<int>("FuncionariosId");
            }
            if (tabela.Table.Columns.Contains("QuantidadeTotal"))
            {
                this.QuantidadeTotal = tabela.Field<int>("QuantidadeTotal");
            }
            if (tabela.Table.Columns.Contains("DataPedido"))
            {
                this.DataPedido = tabela.Field<DateTime>("DataPedido");
            }
            if (tabela.Table.Columns.Contains("Estado"))
            {
                this.Estado = tabela.Field<int>("Estado");
            }
            if (tabela.Table.Columns.Contains("DataConclusao"))
            {
                this.DataConclusao = tabela.Field<DateTime?>("DataConclusao");
            }
        }
        #endregion

        #region Outros Métodos

        /// <summary>
        /// Método para obter a lista de pedidos de material de acordo com o filtro.
        /// </summary>
        /// <param name="filtros">Filtro de parâmetros.</param>
        /// <returns>Devolve a lista de pedidos de material.</returns>
        public static PedidosMaterial[] ObterLista(Dictionary<String, Object> filtros)
        {
            string sql;
            PreparaSQL(filtros, out sql);

            PedidosMaterial[] lstP = Geral<PedidosMaterial>.ObterLista(sql);

            return lstP;
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
            sql = @"SELECT Id, MateriaisId, FuncionariosId, QuantidadeTotal, DataPedido, Estado, DataConclusao FROM PedidosMaterial WHERE 1=1 ";

            // Adicionar filtros à SQL e registar os parâmetros
            if (filtros != null)
            {
                if (filtros.ContainsKey("IdDe") && !string.IsNullOrEmpty(filtros["IdDe"].ToString()))
                {
                    sql += " AND Id >= " + filtros["IdDe"].ToString();
                }
                if (filtros.ContainsKey("IdAte") && !string.IsNullOrEmpty(filtros["IdAte"].ToString()))
                {
                    sql += " AND Id <= " + filtros["IdAte"].ToString();
                }

                // Adicione mais filtros conforme necessário
            }
        }

        public static int Inserir(PedidosMaterial p)
        {
            string sql;
            sql = "INSERT INTO PedidosMaterial (MateriaisId, FuncionariosId, QuantidadeTotal, DataPedido, Estado, DataConclusao) VALUES (" + p.MateriaisId + ", " + p.FuncionariosId + ", " + p.QuantidadeTotal + ", '" + p.DataPedido.ToString("yyyy-MM-dd") + "', " + p.Estado + ", " + (p.DataConclusao != null ? "'" + p.DataConclusao.Value.ToString("yyyy-MM-dd") + "'" : "NULL") + ")";

            return Geral.Geral.Manipular(sql);
        }


        public static int Remover(int i)
        {
            string sql;
            sql = "DELETE FROM PedidosMaterial WHERE Id = " + i;
            return Geral.Geral.Manipular(sql);
        }

        public static int AlterarDados(PedidosMaterial p)
        {
            string sql;
            sql = "UPDATE PedidosMaterial SET MateriaisId = " + p.MateriaisId + ", FuncionariosId = " + p.FuncionariosId + ", QuantidadeTotal = " + p.QuantidadeTotal + ", DataPedido = '" + p.DataPedido.ToString("yyyy-MM-dd") + "', Estado = " + p.Estado + ", DataConclusao = " + (p.DataConclusao != null ? "'" + p.DataConclusao.Value.ToString("yyyy-MM-dd") + "'" : "NULL") + " WHERE Id = " + p.Id;

            return Geral.Geral.Manipular(sql);
        }

        #endregion

        #endregion
    }
}
