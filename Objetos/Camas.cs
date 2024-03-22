/*
*	<copyright file="Sobremesas" company="IPCA"></copyright>
* 	<author>Sofia Carvalho</author>
*	<contact>a25991@alunos.ipca.pt</contact>
*   <date>3/20/2024 18:55:26 PM</date>
*	<description></description>
**/

using Geral;
using System.Collections.Generic;
using System;
using System.Data;

namespace Objetos
{
    public class Camas
    {
        #region atributos
        public int UtentesId { get; set; }
        public int QuartosId { get; set; }
        public int Id { get; set; }

        #endregion

        #region Métodos 
        #region Construtores
        public Camas() { }

        /// <summary>
        /// Construtor para Camas
        /// Recebe uma tabela com dados e de acordo com as colunas vai adicionar ao objeto.
        /// </summary>
        /// <param name="tabela"> Tabela de dados. </param>
        public Camas(DataRow tabela)
        {
            if (tabela.Table.Columns.Contains("UtentesId"))
            {
                this.UtentesId = tabela.Field<int>("UtentesId");
            }
            if (tabela.Table.Columns.Contains("QuartosId"))
            {
                this.QuartosId = tabela.Field<int>("QuartosId");
            }
            if (tabela.Table.Columns.Contains("Id"))
            {
                this.Id = tabela.Field<int>("Id");
            }
        }

        #endregion
        #region Outros Métodos
        /// <summary>
        /// Método para obter a lista de camas de acordo com o filtro.
        /// </summary>
        /// <param name="filtros">Filtro de parámetros.</param>
        /// <returns>Devolve a lista de camas.</returns>
        public static Camas[] ObterLista(Dictionary<String, Object> filtros)
        {
            string sql;
            PreparaSQL(filtros, out sql);

            Camas[] lstS = Geral<Camas>.ObterLista(sql);

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
            }
        }

        public static int Inserir(Camas s)
        {
            string sql;
            sql = "Insert into Avaliacoes (UtentesId, QuartosId, Id) Values (" + s.UtentesId.ToString() + ", '" + s.QuartosId.ToString() + ", '" + s.Id.ToString() + "')";

            return Geral.Geral.Manipular(sql);
        }

        public static int Remover(int i)
        {
            string sql;
            sql = "Delete from Camas where UtenteId = " + i.ToString();
            return Geral.Geral.Manipular(sql);
        }

        public static int AlterarDados(Camas s)
        {
            string sql;
            sql = "Update Camas set QuartosId = '" + s.QuartosId.ToString() + "', Id = '" + s.Id.ToString() + "'";

            return Geral.Geral.Manipular(sql);
        }
        
        #endregion
        #endregion
    }
}