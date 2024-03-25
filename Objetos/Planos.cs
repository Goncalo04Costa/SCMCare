/*
*	<copyright file="Planos" company="IPCA">
*	</copyright>
* 	<author>Gonçalo Costa</author>
*	<contact>a26024@alunos.ipca.pt</contact>
*   <date>2024 24/03/2024 11:45:37</date>
*	<description></description>
**/

using System;
using System.Collections.Generic;
using System.Data;
using MetodosGlobais;

namespace ObjetosNegocio
{
    public class Planos
    {
        #region Atributos

        public int Id { get; set; }
        public int UtentesId { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public string Observacoes { get; set; }

        #endregion

        #region Métodos

        #region Construtores
        public Planos() { }

        /// <summary>
        /// Construtor para planos.
        /// Recebe uma tabela com dados e de acordo com as colunas vai adicionar ao objeto.
        /// </summary>
        /// <param name="tabela">Tabela de dados.</param>
        public Planos(DataRow tabela)
        {
            if (tabela.Table.Columns.Contains("Id"))
            {
                this.Id = tabela.Field<int>("Id");
            }
            if (tabela.Table.Columns.Contains("UtentesId"))
            {
                this.UtentesId = tabela.Field<int>("UtentesId");
            }
            if (tabela.Table.Columns.Contains("DataInicio"))
            {
                this.DataInicio = tabela.Field<DateTime>("DataInicio");
            }
            if (tabela.Table.Columns.Contains("DataFim"))
            {
                this.DataFim = tabela.Field<DateTime?>("DataFim");
            }
            if (tabela.Table.Columns.Contains("Observacoes"))
            {
                this.Observacoes = tabela.Field<string>("Observacoes");
            }
        }
        #endregion

        #region Outros Métodos

        /// <summary>
        /// Método para obter a lista de planos de acordo com o filtro.
        /// </summary>
        /// <param name="filtros">Filtro de parâmetros.</param>
        /// <returns>Devolve a lista de planos.</returns>
        public static List<Planos> ObterLista(Dictionary<String, Object> filtros)
        {
            string sql;
            PreparaSQL(filtros, out sql);

            List<Planos> lstP = Geral<Planos>.ObterLista(sql);

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
            sql = @"SELECT Id, UtentesId, DataInicio, DataFim, Observacoes FROM Planos WHERE 1=1 ";

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

        public static int Inserir(Planos p)
        {
            string sql;
            sql = "INSERT INTO Planos (UtentesId, DataInicio, DataFim, Observacoes) VALUES (" + p.UtentesId + ", '" + p.DataInicio.ToString("yyyy-MM-dd") + "', " + (p.DataFim != null ? "'" + p.DataFim.Value.ToString("yyyy-MM-dd") + "'" : "NULL") + ", '" + p.Observacoes + "')";

            return Geral.Manipular(sql);
        }


        public static int Remover(int i)
        {
            string sql;
            sql = "DELETE FROM Planos WHERE Id = " + i;
            return Geral.Manipular(sql);
        }

        public static int AlterarDados(Planos p)
        {
            string sql;
            sql = "UPDATE Planos SET UtentesId = " + p.UtentesId + ", DataInicio = '" + p.DataInicio.ToString("yyyy-MM-dd") + "', DataFim = " + (p.DataFim != null ? "'" + p.DataFim.Value.ToString("yyyy-MM-dd") + "'" : "NULL") + ", Observacoes = '" + p.Observacoes + "' WHERE Id = " + p.Id;

            return Geral.Manipular(sql);
        }

        #endregion

        #endregion
    }
}

