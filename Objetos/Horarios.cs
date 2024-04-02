/*
*	<copyright file="Horarios" company="IPCA"></copyright>
* 	<author>Sofia Carvalho</author>
*	<contact>a25991@alunos.ipca.pt</contact>
*   <date>3/20/2024 23:24:32 PM</date>
*	<description></description>
**/

using MetodosGlobais;
using System;
using System.Collections.Generic;
using System.Data;

namespace ObjetosNegocio
{
    public class Horarios
    {
        #region atributos
        public int FuncionariosId { get; set; }
        public int TurnosId { get; set; }
        public DateTime Dia { get; set; }

        #endregion
        #region Métodos 
        #region Construtores
        public Horarios() { }

        /// <summary>
        /// Construtor para Horarios
        /// Recebe uma tabela com dados e de acordo com as colunas vai adicionar ao objeto.
        /// </summary>
        /// <param name="tabela"> Tabela de dados. </param>
        public Horarios(DataRow tabela)
        {
            if (tabela.Table.Columns.Contains("FuncionariosId"))
            {
                this.FuncionariosId = tabela.Field<int>("FuncionariosId");
            }
            if (tabela.Table.Columns.Contains("TurnosId"))
            {
                this.TurnosId = tabela.Field<int>("TurnosId");
            }
            if (tabela.Table.Columns.Contains("Dia"))
            {
                this.Dia = tabela.Field<DateTime>("Dia");
            }
        }

        #endregion
        #region Outros Métodos

        /// <summary>
        /// Método para obter a lista de horarios de acordo com o filtro.
        /// </summary>
        /// <param name="filtros">Filtro de parámetros.</param>
        /// <returns>Devolve a lista de horarios.</returns>
        public static List<Horarios> ObterLista(Dictionary<String, Object> filtros)
        {
            string sql;
            PreparaSQL(filtros, out sql);

            List<Horarios> lstS = Geral<Horarios>.ObterLista(sql);

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

            if (filtros != null)
            {
                if (filtros.ContainsKey("IdDe") && !string.IsNullOrEmpty(filtros["IdDe"].ToString()))
                {
                    sql += " and Id >= " + filtros["IdDe"].ToString();
                }
                if (filtros.ContainsKey("IdAte") && !string.IsNullOrEmpty(filtros["IdAte"].ToString()))
                {
                    sql += " and Id <= @" + filtros["IdAte"].ToString();
                }
            
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

        public static int Inserir(Horarios s)
        {
            string sql;
            sql = "Insert into Horarios (FuncionariosId, TurnosId, Dia) Values (" + s.FuncionariosId.ToString() + ", '" + s.TurnosId.ToString() + ", '" + s.Dia.ToString() + "')";

            return Geral.Manipular(sql);
        }

        public static int Remover(int i, int a, DateTime b)
        {
            string sql;
            sql = "Delete from Horarios where FuncionariosId = " + i.ToString() + " and TurnosId = " + a.ToString() + " and Dia = " + b.ToString();
            return Geral.Manipular(sql);
        }

        public static int AlterarDados(Horarios s)
        {
            string sql;
            sql = "Update Horarios set FuncionariosId = '" + s.FuncionariosId.ToString() + "', TurnosId = '" + s.TurnosId.ToString() + "', Dia = '" + s.Dia.ToString() + "'";

            return Geral.Manipular(sql);
        }


     


        #endregion
        #endregion
    }
}
