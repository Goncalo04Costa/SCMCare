/*
*	<copyright file="Sobremesas" company="IPCA"></copyright>
* 	<author>Sofia Carvalho</author>
*	<contact>a25991@alunos.ipca.pt</contact>
*   <date>3/20/2024 21:58:43 PM</date>
*	<description></description>
**/

using Geral;
using System;
using System.Collections.Generic;
using System.Data;

namespace Objetos
{
    public class FeriasFuncionario
    {
        #region atributos
        public int Id { get; set; }
        public int FuncionariosId { get; set; }
        public int FuncionariosIdValida { get; set; }
        public DateTime Dia { get; set; }
        public int Estado { get; set; }

        #endregion
        #region Métodos 
        #region Construtores
        public FeriasFuncionario() { }

        /// <summary>
        /// Construtor para FeriasFuncionario
        /// Recebe uma tabela com dados e de acordo com as colunas vai adicionar ao objeto.
        /// </summary>
        /// <param name="tabela"> Tabela de dados. </param>
        public FeriasFuncionario(DataRow tabela)
        {
            if (tabela.Table.Columns.Contains("Id"))
            {
                this.Id = tabela.Field<int>("Id");
            }
            if (tabela.Table.Columns.Contains("FuncionariosId"))
            {
                this.FuncionariosId = tabela.Field<int>("FuncionariosId");
            }
            if (tabela.Table.Columns.Contains("FuncionariosIdValida"))
            {
                this.FuncionariosIdValida = tabela.Field<int>("FuncionariosIdValida");
            }
            if (tabela.Table.Columns.Contains("Dia"))
            {
                this.Dia = tabela.Field<DateTime>("Dia");
            }
            if (tabela.Table.Columns.Contains("Estado"))
            {
                this.Estado = tabela.Field<int>("Estado");
            }
        }

        #endregion
        #region Outros Métodos

        /// <summary>
        /// Método para obter a lista de ferias dos funcionarios de acordo com o filtro.
        /// </summary>
        /// <param name="filtros">Filtro de parámetros.</param>
        /// <returns>Devolve a lista de ferias dos funcionarios.</returns>
        public static FeriasFuncionario[] ObterLista(Dictionary<String, Object> filtros)
        {
            string sql;
            PreparaSQL(filtros, out sql);

            FeriasFuncionario[] lstS = Geral<FeriasFuncionario>.ObterLista(sql);

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

        public static int Inserir(FeriasFuncionario s)
        {
            string sql;
            sql = "Insert into FeriasFuncionario (FuncionariosId, FuncionariosIdValida, Dia, Estado) Values (" + s.FuncionariosId.ToString() + ", '" + s.FuncionariosIdValida.ToString() + ", '" + s.Dia.ToString() + "', '" + s.Estado.ToString() + "')";

            return Geral.Geral.Manipular(sql);
        }

        public static int Remover(int i)
        {
            string sql;
            sql = "Delete from FeriasFuncionario where Id = " + i.ToString();
            return Geral.Geral.Manipular(sql);
        }

        public static int AlterarDados(FeriasFuncionario s)
        {
            string sql;
            sql = "Update FeriasFuncionario set FuncionariosId = '" + s.FuncionariosId.ToString() + "', FuncionariosIdValida = '" + s.FuncionariosIdValida.ToString() + "', Dia = '" + s.Dia.ToString() + "', Estado = '" + s.Estado.ToString() + "'";

            return Geral.Geral.Manipular(sql);
        }

        #endregion
        #endregion
    }
}