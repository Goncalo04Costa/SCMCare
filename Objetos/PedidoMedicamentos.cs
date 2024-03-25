/*
*	<copyright file="PedidoMedicamentos" company="IPCA">
*	</copyright>
* 	<author>Gonçalo Costa</author>
*	<contact>a26024@alunos.ipca.pt</contact>
*   <date>2024 24/03/2024 11:42:29</date>
*	<description></description>
**/

using System;
using System.Collections.Generic;
using System.Data;
using MetodosGlobais;

namespace ObjetosNegocio
{
    public class PedidosMedicamento
    {
        #region Atributos

        public int Id { get; set; }
        public int MedicamentosId { get; set; }
        public int FuncionariosId { get; set; }
        public int Quantidade { get; set; }
        public DateTime DataPedido { get; set; }
        public int Estado { get; set; }
        public DateTime? DataConclusao { get; set; }

        #endregion

        #region Métodos

        #region Construtores
        public PedidosMedicamento() { }

        /// <summary>
        /// Construtor para pedidos de medicamento.
        /// Recebe uma tabela com dados e de acordo com as colunas vai adicionar ao objeto.
        /// </summary>
        /// <param name="tabela">Tabela de dados.</param>
        public PedidosMedicamento(DataRow tabela)
        {
            if (tabela.Table.Columns.Contains("Id"))
            {
                this.Id = tabela.Field<int>("Id");
            }
            if (tabela.Table.Columns.Contains("MedicamentosId"))
            {
                this.MedicamentosId = tabela.Field<int>("MedicamentosId");
            }
            if (tabela.Table.Columns.Contains("FuncionariosId"))
            {
                this.FuncionariosId = tabela.Field<int>("FuncionariosId");
            }
            if (tabela.Table.Columns.Contains("Quantidade"))
            {
                this.Quantidade = tabela.Field<int>("Quantidade");
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
        /// Método para obter a lista de pedidos de medicamento de acordo com o filtro.
        /// </summary>
        /// <param name="filtros">Filtro de parâmetros.</param>
        /// <returns>Devolve a lista de pedidos de medicamento.</returns>
        public static List<PedidosMedicamento> ObterLista(Dictionary<String, Object> filtros)
        {
            string sql;
            PreparaSQL(filtros, out sql);

            List<PedidosMedicamento> lstP = Geral<PedidosMedicamento>.ObterLista(sql);

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
            sql = @"SELECT Id, MedicamentosId, FuncionariosId, Quantidade, DataPedido, Estado, DataConclusao FROM PedidosMedicamento WHERE 1=1 ";

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

        public static int Inserir(PedidosMedicamento p)
        {
            string sql;
            sql = "INSERT INTO PedidosMedicamento (MedicamentosId, FuncionariosId, Quantidade, DataPedido, Estado, DataConclusao) VALUES (" + p.MedicamentosId + ", " + p.FuncionariosId + ", " + p.Quantidade + ", '" + p.DataPedido.ToString("yyyy-MM-dd") + "', " + p.Estado + ", " + (p.DataConclusao != null ? "'" + p.DataConclusao.Value.ToString("yyyy-MM-dd") + "'" : "NULL") + ")";

            return Geral.Manipular(sql);
        }


        public static int Remover(int i)
        {
            string sql;
            sql = "DELETE FROM PedidosMedicamento WHERE Id = " + i;
            return Geral.Manipular(sql);
        }

        public static int AlterarDados(PedidosMedicamento p)
        {
            string sql;
            sql = "UPDATE PedidosMedicamento SET MedicamentosId = " + p.MedicamentosId + ", FuncionariosId = " + p.FuncionariosId + ", Quantidade = " + p.Quantidade + ", DataPedido = '" + p.DataPedido.ToString("yyyy-MM-dd") + "', Estado = " + p.Estado + ", DataConclusao = " + (p.DataConclusao != null ? "'" + p.DataConclusao.Value.ToString("yyyy-MM-dd") + "'" : "NULL") + " WHERE Id = " + p.Id;

            return Geral.Manipular(sql);
        }

        #endregion

        #endregion
    }
}
