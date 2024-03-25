/*
*	<copyright file="Hospitais" company="IPCA">
*	</copyright>
* 	<author>Gonçalo Costa</author>
*	<contact>a26024@alunos.ipca.pt</contact>
*   <date>2024 25/03/2024 10:50:16</date>
*	<description></description>
**/

using Geral;
using System.Collections.Generic;
using System.Data;
using System;

namespace Objetos
{
    public class Hospitais
    {
        #region Atributos

        public int Id { get; set; }
        public string Nome { get; set; }
        public string Morada { get; set; }

        #endregion

        #region Métodos

        #region Construtores
        public Hospitais() { }

        /// <summary>
        /// Construtor para hospitais.
        /// Recebe uma tabela com dados e de acordo com as colunas vai adicionar ao objeto.
        /// </summary>
        /// <param name="tabela">Tabela de dados.</param>
        public Hospitais(DataRow tabela)
        {
            if (tabela.Table.Columns.Contains("Id"))
            {
                this.Id = tabela.Field<int>("Id");
            }
            if (tabela.Table.Columns.Contains("Nome"))
            {
                this.Nome = tabela.Field<string>("Nome");
            }
            if (tabela.Table.Columns.Contains("Morada"))
            {
                this.Morada = tabela.Field<string>("Morada");
            }
        }
        #endregion

        #region Outros Métodos

        /// <summary>
        /// Método para obter a lista de hospitais de acordo com o filtro.
        /// </summary>
        /// <param name="filtros">Filtro de parâmetros.</param>
        /// <returns>Devolve a lista de hospitais.</returns>
        public static Hospitais[] ObterLista(Dictionary<String, Object> filtros)
        {
            string sql;
            PreparaSQL(filtros, out sql);

            Hospitais[] lstS = Geral<Hospitais>.ObterLista(sql);

            return lstS;
        }

        /// <summary>
        /// Método para preparar a query sql com os filtros obtidos.
        /// </summary>
        /// <param name="filtros">Filtros a aplicar.</param>
        /// <param name="sql">Query sql.</param>
        private static void PreparaSQL(Dictionary<String, Object> filtros, out string sql)
        {
            // Parâmetros a devolver no final
            List<object> parSQL = new List<object>();
            sql = @"Select Id, Nome, Morada From Hospitais where 1=1 ";

            // Adicionar filtros ao sql, e registrar os parâmetros
            if (filtros != null)
            {
                // Para int - Aplica filtro para um intervalo de Ids.
                if (filtros.ContainsKey("IdDe") && !string.IsNullOrEmpty(filtros["IdDe"].ToString()))
                {
                    sql += " and Id >= " + filtros["IdDe"].ToString();
                }
                if (filtros.ContainsKey("IdAte") && !string.IsNullOrEmpty(filtros["IdAte"].ToString()))
                {
                    sql += " and Id <= @" + filtros["IdAte"].ToString();
                }

                //  Para string - Verifica se existe alguma string como a recebida no filtro (ignorando a capitalização e acentuação)
                if (filtros.ContainsKey("Nome") && !string.IsNullOrEmpty(filtros["Nome"].ToString()))
                {
                    sql += " and Nome COLLATE Latin1_general_CI_AI LIKE '%" + filtros["Nome"].ToString() + "%' COLLATE Latin1_general_CI_AI";
                }
            }
        }

        public static int Inserir(Hospitais h)
        {
            string sql;
            sql = "Insert into Hospitais (Nome, Morada) Values ('" + h.Nome.ToString() + "', '" + h.Morada.ToString() + "')";

            return Geral.Geral.Manipular(sql);
        }

        public static int Remover(int id)
        {
            string sql = "DELETE FROM Hospitais WHERE Id = " + id;
            return Geral.Geral.Manipular(sql);
        }

        #endregion

        #endregion
    }
}