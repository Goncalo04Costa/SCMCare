/*
*	<copyright file="Responsaveis" company="IPCA">
*	</copyright>
* 	<author>Gonçalo Costa</author>
*	<contact>a26024@alunos.ipca.pt</contact>
*   <date>2024 24/03/2024 11:52:49</date>
*	<description></description>
**/

using System;
using System.Collections.Generic;
using System.Data;
using MetodosGlobais;

namespace ObjetosNegocio
{
    public class Responsaveis
    {
        #region Atributos

        public int Id { get; set; }
        public string Nome { get; set; }
        public int UtentesId { get; set; }
        public string Morada { get; set; }

        #endregion

        #region Métodos

        #region Construtores
        public Responsaveis() { }

        /// <summary>
        /// Construtor para responsáveis.
        /// Recebe uma tabela com dados e de acordo com as colunas vai adicionar ao objeto.
        /// </summary>
        /// <param name="tabela">Tabela de dados.</param>
        public Responsaveis(DataRow tabela)
        {
            if (tabela.Table.Columns.Contains("Id"))
            {
                this.Id = tabela.Field<int>("Id");
            }
            if (tabela.Table.Columns.Contains("Nome"))
            {
                this.Nome = tabela.Field<string>("Nome");
            }
            if (tabela.Table.Columns.Contains("UtentesId"))
            {
                this.UtentesId = tabela.Field<int>("UtentesId");
            }
            if (tabela.Table.Columns.Contains("Morada"))
            {
                this.Morada = tabela.Field<string>("Morada");
            }
        }
        #endregion

        #region Outros Métodos

        /// <summary>
        /// Método para obter a lista de responsáveis de acordo com o filtro.
        /// </summary>
        /// <param name="filtros">Filtro de parâmetros.</param>
        /// <returns>Devolve a lista de responsáveis.</returns>
        public static List<Responsaveis> ObterLista(Dictionary<String, Object> filtros)
        {
            string sql;
            PreparaSQL(filtros, out sql);

            List<Responsaveis> lstR = Geral<Responsaveis>.ObterLista(sql);

            return lstR;
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
            sql = @"SELECT Id, Nome, UtentesId, Morada FROM Responsaveis WHERE 1=1 ";

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

        public static int Inserir(Responsaveis r)
        {
            string sql;
            sql = "INSERT INTO Responsaveis (Nome, UtentesId, Morada) VALUES ('" + r.Nome + "', " + r.UtentesId + ", '" + r.Morada + "')";

            return Geral.Manipular(sql);
        }


        public static int Remover(int i)
        {
            string sql;
            sql = "DELETE FROM Responsaveis WHERE Id = " + i;
            return Geral.Manipular(sql);
        }

        public static int AlterarDados(Responsaveis r)
        {
            string sql;
            sql = "UPDATE Responsaveis SET Nome = '" + r.Nome + "', UtentesId = " + r.UtentesId + ", Morada = '" + r.Morada + "' WHERE Id = " + r.Id;

            return Geral.Manipular(sql);
        }

        #endregion

        #endregion
    }
}
