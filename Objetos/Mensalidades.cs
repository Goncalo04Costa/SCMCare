/*
*	<copyright file="Mensalidades" company="IPCA">
*	</copyright>
* 	<author>Gonçalo Costa</author>
*	<contact>a26024@alunos.ipca.pt</contact>
*   <date>2024 24/03/2024 12:04:03</date>
*	<description></description>
**/

using System;
using System.Collections.Generic;
using System.Data;
using Geral;

namespace Objetos
{
    public class Mensalidades
    {
        #region Atributos

        public DateTime Mes { get; set; }
        public DateTime? DataPagamento { get; set; }
        public int UtentesId { get; set; }
        public int? TiposPagamentoId { get; set; }
        public int Estado { get; set; }

        #endregion

        #region Métodos

        #region Construtores
        public Mensalidades() { }

        /// <summary>
        /// Construtor para mensalidades.
        /// Recebe uma tabela com dados e de acordo com as colunas vai adicionar ao objeto.
        /// </summary>
        /// <param name="tabela">Tabela de dados.</param>
        public Mensalidades(DataRow tabela)
        {
            if (tabela.Table.Columns.Contains("Mes"))
            {
                this.Mes = tabela.Field<DateTime>("Mes");
            }
            if (tabela.Table.Columns.Contains("DataPagamento"))
            {
                this.DataPagamento = tabela.Field<DateTime?>("DataPagamento");
            }
            if (tabela.Table.Columns.Contains("UtentesId"))
            {
                this.UtentesId = tabela.Field<int>("UtentesId");
            }
            if (tabela.Table.Columns.Contains("TiposPagamentoId"))
            {
                this.TiposPagamentoId = tabela.Field<int?>("TiposPagamentoId");
            }
            if (tabela.Table.Columns.Contains("Estado"))
            {
                this.Estado = tabela.Field<int>("Estado");
            }
        }
        #endregion

        #region Outros Métodos

        /// <summary>
        /// Método para obter a lista de mensalidades de acordo com o filtro.
        /// </summary>
        /// <param name="filtros">Filtro de parâmetros.</param>
        /// <returns>Devolve a lista de mensalidades.</returns>
        public static Mensalidades[] ObterLista(Dictionary<String, Object> filtros)
        {
            string sql;
            PreparaSQL(filtros, out sql);

            Mensalidades[] lstM = Geral<Mensalidades>.ObterLista(sql);

            return lstM;
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
            sql = @"SELECT Mes, DataPagamento, UtentesId, TiposPagamentoId, Estado FROM Mensalidades WHERE 1=1 ";

            // Adicionar filtros à SQL e registar os parâmetros
            if (filtros != null)
            {
                // Adicione mais filtros conforme necessário
            }
        }

        public static int Inserir(Mensalidades m)
        {
            string sql;
            sql = "INSERT INTO Mensalidades (Mes, DataPagamento, UtentesId, TiposPagamentoId, Estado) VALUES ('" + m.Mes.ToString("yyyy-MM-dd") + "', " + (m.DataPagamento != null ? "'" + m.DataPagamento.Value.ToString("yyyy-MM-dd") + "'" : "NULL") + ", " + m.UtentesId + ", " + (m.TiposPagamentoId != null ? m.TiposPagamentoId.ToString() : "NULL") + ", " + m.Estado + ")";

            return Geral.Geral.Manipular(sql);
        }


        public static int Remover(DateTime mes, int utentesId)
        {
            string sql;
            sql = "DELETE FROM Mensalidades WHERE Mes = '" + mes.ToString("yyyy-MM-dd") + "' AND UtentesId = " + utentesId;
            return Geral.Geral.Manipular(sql);
        }

        #endregion

        #endregion
    }
}
