/*
*	<copyright file="Alta" company="IPCA"></copyright>
* 	<author>Sofia Carvalho</author>
*	<contact>a25991@alunos.ipca.pt</contact>
*   <date>3/20/2024 16:07:29 PM</date>
*	<description></description>
**/

using MetodosGlobais;
using System;
using System.Collections.Generic;
using System.Data;

namespace ObjetosNegocio
{
    public class Alta
    {
        #region atributos
        public int UtentesId { get; set; }
        public int FuncionariosId { get; set; }
        public string Motivo { get; set; }
        public string Destino { get; set; }

        #endregion

        #region Métodos
        #region Construtores
        public Alta() { }

        /// <summary>
        /// Construtos para Alta
        /// Recebe uma tabela com dados e de acordo com as colunas vai adicionar ao objeto.
        /// </summary>
        /// <param name="tabela"> Tabela de dados. </param>
        public Alta(DataRow tabela)
        {
            if (tabela.Table.Columns.Contains("UtentesId")) {
                this.UtentesId = tabela.Field<int>("UtentesId");
            }
            if (tabela.Table.Columns.Contains("FuncionariosId"))
            {
                this.FuncionariosId = tabela.Field<int>("FuncionariosId");
            }
            if (tabela.Table.Columns.Contains("Motivo"))
            {
                this.Motivo = tabela.Field<string>("Motivo");
            }
            if (tabela.Table.Columns.Contains("Destino"))
            {
                this.Destino = tabela.Field<string>("Destino");
            }
        }
        #endregion
        #region Outros Métodos
        /// <summary>
        /// Método para obter a lista de altas de acordo com o filtro.
        /// </summary>
        /// <param name="filtros">Filtro de parámetros.</param>
        /// <returns>Devolve a lista de altas.</returns>
        public static List<Alta> ObterLista(Dictionary<String, Object> filtros)
        {
            string sql;
            PreparaSQL(filtros, out sql);

            List<Alta> lstS = Geral<Alta>.ObterLista(sql);

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
            sql = @"Select UtentesId, FuncionariosId, Motivo, Destino From Alta where 1=1";

            // Adicionar filtros ao sql, e registar os parámetros
            if(filtros!= null)
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

        public static int Inserir(Alta s)
        {
            string sql;
            sql = "Insert into Alta (UtentesId, FuncionariosId, Motivo, Destino) Values (" + s.UtentesId.ToString() + ", '" + s.FuncionariosId.ToString() + ", '" + s.Motivo + "', '" + s.Destino + "')";

            return Geral.Manipular(sql);
        }

        public static int Remover(int i)
        {
            string sql;
            sql = "Delete from Alta where UtentesId = " + i.ToString();
            return Geral.Manipular(sql);
        }

        public static int AlterarDados(Alta s)
        {
            string sql;
            sql = "Update Alta set Motivo = '" + s.Motivo + "', Destino = '" + s.Destino + "'";

            return Geral.Manipular(sql);
        }

        #endregion
        #endregion
    }
}
