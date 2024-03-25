/*
*	<copyright file="Quartos" company="IPCA">
*	</copyright>
* 	<author>Gonçalo Costa</author>
*	<contact>a26024@alunos.ipca.pt</contact>
*   <date>2024 24/03/2024 11:52:13</date>
*	<description></description>
**/

using System;
using System.Collections.Generic;
using System.Data;
using MetodosGlobais;

namespace ObjetosNegocio
{
    public class Quartos
    {
        #region Atributos

        public int Id { get; set; }
        public int Numero { get; set; }
        public int TiposQuartoId { get; set; }

        #endregion

        #region Métodos

        #region Construtores
        public Quartos() { }

        /// <summary>
        /// Construtor para quartos.
        /// Recebe uma tabela com dados e de acordo com as colunas vai adicionar ao objeto.
        /// </summary>
        /// <param name="tabela">Tabela de dados.</param>
        public Quartos(DataRow tabela)
        {
            if (tabela.Table.Columns.Contains("Id"))
            {
                this.Id = tabela.Field<int>("Id");
            }
            if (tabela.Table.Columns.Contains("Numero"))
            {
                this.Numero = tabela.Field<int>("Numero");
            }
            if (tabela.Table.Columns.Contains("TiposQuartoId"))
            {
                this.TiposQuartoId = tabela.Field<int>("TiposQuartoId");
            }
        }
        #endregion

        #region Outros Métodos

        /// <summary>
        /// Método para obter a lista de quartos de acordo com o filtro.
        /// </summary>
        /// <param name="filtros">Filtro de parâmetros.</param>
        /// <returns>Devolve a lista de quartos.</returns>
        public static List<Quartos> ObterLista(Dictionary<String, Object> filtros)
        {
            string sql;
            PreparaSQL(filtros, out sql);

            List<Quartos> lstQ = Geral<Quartos>.ObterLista(sql);

            return lstQ;
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
            sql = @"SELECT Id, Numero, TiposQuartoId FROM Quartos WHERE 1=1 ";

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

        public static int Inserir(Quartos q)
        {
            string sql;
            sql = "INSERT INTO Quartos (Numero, TiposQuartoId) VALUES (" + q.Numero + ", " + q.TiposQuartoId + ")";

            return Geral.Manipular(sql);
        }


        public static int Remover(int i)
        {
            string sql;
            sql = "DELETE FROM Quartos WHERE Id = " + i;
            return Geral.Manipular(sql);
        }

        public static int AlterarDados(Quartos q)
        {
            string sql;
            sql = "UPDATE Quartos SET Numero = " + q.Numero + ", TiposQuartoId = " + q.TiposQuartoId + " WHERE Id = " + q.Id;

            return Geral.Manipular(sql);
        }

        #endregion

        #endregion
    }
}
