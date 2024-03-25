/*
*	<copyright file="Avaliacoes" company="IPCA"></copyright>
* 	<author>Sofia Carvalho</author>
*	<contact>a25991@alunos.ipca.pt</contact>
*   <date>3/20/2024 17:57:35 PM</date>
*	<description></description>
**/

using MetodosGlobais;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices.ComTypes;

namespace ObjetosNegocio
{
    public class Avaliacoes
    {
        #region atributos
        public int Id { get; set; }
        public int UtentesId { get; set; }
        public int FuncionariosId { get; set; }
        public string Analise { get; set; }
        public DateTime Data { get; set; }
        public int TipoAvaliacaoId { get; set; }
        public string AuscultacaoPolmunar { get; set; }
        public string AucultacaoCardiaca { get; set; }
        #endregion

        #region Métodos 
        #region Construtores
        public Avaliacoes() { }

        /// <summary>
        /// Construtos para Avaliacoes
        /// Recebe uma tabela com dados e de acordo com as colunas vai adicionar ao objeto.
        /// </summary>
        /// <param name="tabela"> Tabela de dados. </param>
        public Avaliacoes(DataRow tabela)
        {
            if (tabela.Table.Columns.Contains("Id"))
            {
                this.Id = tabela.Field<int>("Id");
            }
            if (tabela.Table.Columns.Contains("UtentesId"))
            {
                this.UtentesId = tabela.Field<int>("UtentesId");
            }
            if (tabela.Table.Columns.Contains("FuncionariosId"))
            {
                this.FuncionariosId = tabela.Field<int>("FuncionariosId");
            }
            if (tabela.Table.Columns.Contains("Analise"))
            {
                this.Analise = tabela.Field<string>("Analise");
            }
            if (tabela.Table.Columns.Contains("Data"))
            {
                this.Data = tabela.Field<DateTime>("Data");
            }
            if (tabela.Table.Columns.Contains("TipoAvaliacaoId"))
            {
                this.TipoAvaliacaoId = tabela.Field<int>("TipoAvaliacaoId");
            }
            if (tabela.Table.Columns.Contains("AuscultacaoPolmunar"))
            {
                this.AuscultacaoPolmunar = tabela.Field<string>("AuscultacaoPolmunar");
            }
            if (tabela.Table.Columns.Contains("AucultacaoCardiaca"))
            {
                this.AucultacaoCardiaca = tabela.Field<string>("AucultacaoCardiaca");
            }
        }

        #endregion
        #region Outros Métodos

        /// <summary>
        /// Método para obter a lista de avaliacoes de acordo com o filtro.
        /// </summary>
        /// <param name="filtros">Filtro de parámetros.</param>
        /// <returns>Devolve a lista de avaliacoes.</returns>
        public static List<Avaliacoes> ObterLista(Dictionary<String, Object> filtros)
        {
            string sql;
            PreparaSQL(filtros, out sql);

            List<Avaliacoes> lstS = Geral<Avaliacoes>.ObterLista(sql);

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
            sql = @"Select Id, UtentesId, FuncionariosId, Analise, Data, TipoAvaliacaoId, AuscultacaoPolmunar, AucultacaoCardiaca From Avaliacao where 1=1";

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
                if(filtros.ContainsKey("DataDe") && !string.IsNullOrEmpty(filtros["DataDe"].ToString()))
                {
                    sql += " and Data >= " + filtros["DataDe"].ToString();
                }
                if (filtros.ContainsKey("DataAte") && !string.IsNullOrEmpty(filtros["DataAte"].ToString()))
                {
                    sql += " and Data <= " + filtros["DataAte"].ToString();
                }
            }
        }

        public static int Inserir(Avaliacoes s)
        {
            string sql;
            sql = "Insert into Avaliacoes (UtentesId, FuncionariosId, Analise, Data, TipoAvaliacaoId, AuscultacaoPolmunar, AucultacaoCardiaca) Values (" + s.UtentesId.ToString() + ", '" + s.FuncionariosId.ToString() + ", '" + s.Analise + "', '" + s.Data.ToString() + ", '" + s.TipoAvaliacaoId.ToString() + ", '" + s.AuscultacaoPolmunar + ", '" + s.AucultacaoCardiaca + "')";

            return Geral.Manipular(sql);
        }

        public static int Remover(int i, int a, DateTime d)
        {
            string sql;
            sql = "Delete from Avaliacoes where UtentesId = " + i.ToString() + " and FuncionariosId = " + a.ToString() + " and Data = " + d.ToString();
            return Geral.Manipular(sql);
        }

        public static int AlterarDados(Avaliacoes s)
        {
            string sql;
            sql = "Update Avaliacoes set Analise = '" + s.Analise + "', Data = '" + s.Data.ToString() + "', TipoAvaliacaoId = '" + s.TipoAvaliacaoId + "', AuscultacaoPolmunar = '" + s.AuscultacaoPolmunar + "', AucultacaoCardiaca = '" + s.AucultacaoCardiaca + "'";

            return Geral.Manipular(sql);
        }
        #endregion
        #endregion
    }
}
