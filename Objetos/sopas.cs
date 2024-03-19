/*
*	<copyright file="sopas" company="IPCA">
*	</copyright>
* 	<author>Gonçalo Costa</author>
*	<contact>a26024@alunos.ipca.pt</contact>
*   <date>2024 19/03/2024 22:26:18</date>
*	<description></description>
**/


using Geral;
using System.Collections.Generic;
using System.Data;
using System;

namespace Objetos
{
    public class Sopas
    {
        #region Atributos

        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public bool Tipo { get; set; }

        #endregion

        #region Métodos

        #region Construtores
        public Sopas() { }

        /// <summary>
        /// Construtor para sopas.
        /// Recebe uma tabela com dados e de acordo com as colunas vai adicionar ao objeto.
        /// </summary>
        /// <param name="tabela">Tabela de dados.</param>
        public Sopas(DataRow tabela)
        {
            if (tabela.Table.Columns.Contains("Id"))
            {
                this.Id = tabela.Field<int>("Id");
            }
            if (tabela.Table.Columns.Contains("Nome"))
            {
                this.Nome = tabela.Field<string>("Nome");
            }
            if (tabela.Table.Columns.Contains("Descricao"))
            {
                this.Descricao = tabela.Field<string>("Descricao");
            }
            if (tabela.Table.Columns.Contains("Tipo"))
            {
                this.Tipo = tabela.Field<bool>("Tipo");
            }
        }
        #endregion

        #region Outros Métodos

        /// <summary>
        /// Método para obter a lista de sopas de acordo com o filtro.
        /// </summary>
        /// <param name="filtros">Filtro de parámetros.</param>
        /// <returns>Devolve a lista de sopas.</returns>
        public static Sopas[] ObterLista(Dictionary<String, Object> filtros)
        {
            string sql;
            PreparaSQL(filtros, out sql);

            Sopas[] lstS = Geral<Sopas>.ObterLista(sql);

            return lstS;
        }

        /// <summary>
        /// Método para preparar a query sql com os filtros obtidos.
        /// </summary>
        /// <param name="filtros">Filtros a aplicar.</param>
        /// <param name="sql">Query sql.</param>
        private static void PreparaSQL(Dictionary<String, Object> filtros, out string sql)
        {
            // Parámetros a devolver no final
            List<object> parSQL = new List<object>();
            sql = @"Select Id, Nome, Descricao, Tipo From Sopas where 1=1 ";


            // Adicionar filtros ao sql, e registar os parámetros
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

                // Para bit/bool - Verifica se tem filtro para True, verifica se tem filtro para False, se já tiver filtro para True, adiciona 'or',
                // se a string não estiver vazia adiciona os filtros à string SQL.
                String Tipo = "";
                if (filtros.ContainsKey("Tipo1") && filtros["Tipo1"].ToString() == "1")
                {
                    Tipo += "Tipo=1";
                }
                if (filtros.ContainsKey("Tipo0") && filtros["Tipo0"].ToString() == "1")
                {
                    if (!String.IsNullOrWhiteSpace(Tipo))
                        Tipo += " or ";
                    Tipo += "Tipo=0";
                }
                if (!String.IsNullOrWhiteSpace(Tipo))
                    sql += String.Format(" and ({0})", Tipo);

            }
        }

        public static int Inserir(Sopas s)
        {
            string sql;
            sql = "Insert into Sopas (Nome, Descricao, Tipo) Values ('" + s.Nome.ToString() + "', '" + s.Descricao.ToString() + "', " + Geral.Geral.BoolToInt(s.Tipo) + ")";

            return Geral.Geral.Inserir(sql);
        }

        #endregion

        #endregion
    }
}