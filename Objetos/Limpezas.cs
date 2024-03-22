/*
*	<copyright file="Sobremesas" company="IPCA"></copyright>
* 	<author>Sofia Carvalho</author>
*	<contact>a25991@alunos.ipca.pt</contact>
*   <date>3/20/2024 23:33:23 PM</date>
*	<description></description>
**/

using Geral;
using System;
using System.Collections.Generic;
using System.Data;

namespace Objetos
{
    public class Limpezas
    {
        #region atributos
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public int QuartosId { get; set; }
        public int FuncionariosId { get; set; }

        #endregion
        #region Métodos 
        #region Construtores
        public Limpezas() { }

        /// <summary>
        /// Construtor para Limpezas
        /// Recebe uma tabela com dados e de acordo com as colunas vai adicionar ao objeto.
        /// </summary>
        /// <param name="tabela"> Tabela de dados. </param>
        public Limpezas(DataRow tabela)
        {
            if (tabela.Table.Columns.Contains("Id"))
            {
                this.Id = tabela.Field<int>("Id");
            }
            if (tabela.Table.Columns.Contains("Data"))
            {
                this.Data = tabela.Field<DateTime>("Data");
            }
            if (tabela.Table.Columns.Contains("QuartosId"))
            {
                this.QuartosId = tabela.Field<int>("QuartosId");
            }
            if (tabela.Table.Columns.Contains("FuncionariosId"))
            {
                this.FuncionariosId = tabela.Field<int>("FuncionariosId");
            }
        }

        #endregion
        #region Outros Métodos
        /// <summary>
        /// Método para obter a lista de limpezas de acordo com o filtro.
        /// </summary>
        /// <param name="filtros">Filtro de parámetros.</param>
        /// <returns>Devolve a lista de limpezas.</returns>
        public static Limpezas[] ObterLista(Dictionary<String, Object> filtros)
        {
            string sql;
            PreparaSQL(filtros, out sql);

            Limpezas[] lstS = Geral<Limpezas>.ObterLista(sql);

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
            sql = @"Select UtentesId, FuncionariosId, Analise, Data, TipoAvaliacaoId, AuscultacaoPolmunar, AucultacaoCardiaca From Avaliacao where 1=1";

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
                // Para DateTime - Aplica filtro de data
                if (filtros.ContainsKey("DataDe") && !string.IsNullOrEmpty(filtros["DataDe"].ToString()))
                {
                    sql += " and Data >= " + filtros["DataDe"].ToString();
                }
                if (filtros.ContainsKey("DataAte") && !string.IsNullOrEmpty(filtros["DataAte"].ToString()))
                {
                    sql += " and Data <= " + filtros["DataAte"].ToString();
                }
            }
        }

        public static int Inserir(Limpezas s)
        {
            string sql;
            sql = "Insert into Limpezas (Data, QuartosId, FuncionariosId) Values (" + s.Data.ToString() + ", '" + s.QuartosId.ToString() + ", '" + s.FuncionariosId.ToString() + "')";

            return Geral.Geral.Manipular(sql);
        }

        public static int Remover(int i)
        {
            string sql;
            sql = "Delete from Limpezas where Id = " + i.ToString();
            return Geral.Geral.Manipular(sql);
        }

        public static int AlterarDados(Limpezas s)
        {
            string sql;
            sql = "Update Limpezas set Data = '" + s.Data.ToString() + "', QuartosId = '" + s.QuartosId.ToString() + "', FuncionariosId = '" + s.FuncionariosId.ToString() + "'";

            return Geral.Geral.Manipular(sql);
        }
        #endregion
        #endregion
    }
}