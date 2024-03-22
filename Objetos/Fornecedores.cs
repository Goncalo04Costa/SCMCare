/*
*	<copyright file="Sobremesas" company="IPCA"></copyright>
* 	<author>Sofia Carvalho</author>
*	<contact>a25991@alunos.ipca.pt</contact>
*   <date>3/20/2024 22:09:17 PM</date>
*	<description></description>
**/

using Geral;
using System.Collections.Generic;
using System;
using System.Data;

namespace Objetos
{
    public class Fornecedores
    {
        #region atributos
        public int Id { get; set; }
        public string Nome { get; set; }


        #endregion
        #region Métodos 
        #region Construtores
        public Fornecedores() { }

        /// <summary>
        /// Construtor para Fornecedores
        /// Recebe uma tabela com dados e de acordo com as colunas vai adicionar ao objeto.
        /// </summary>
        /// <param name="tabela"> Tabela de dados. </param>
        public Fornecedores(DataRow tabela)
        {
            if (tabela.Table.Columns.Contains("Id"))
            {
                this.Id = tabela.Field<int>("Id");
            }
            if (tabela.Table.Columns.Contains("Nome"))
            {
                this.Nome = tabela.Field<string>("Nome");
            }
        }

        #endregion
        #region Outros Métodos

        /// <summary>
        /// Método para obter a lista de fornecedores de acordo com o filtro.
        /// </summary>
        /// <param name="filtros">Filtro de parámetros.</param>
        /// <returns>Devolve a lista de fornecedores.</returns>
        public static Fornecedores[] ObterLista(Dictionary<String, Object> filtros)
        {
            string sql;
            PreparaSQL(filtros, out sql);

            Fornecedores[] lstS = Geral<Fornecedores>.ObterLista(sql);

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

                //  Para string - Verifica se existe alguma string como a recebida no filtro (ignorando a capitalização e acentuação)
                if (filtros.ContainsKey("Nome") && !string.IsNullOrEmpty(filtros["Nome"].ToString()))
                {
                    sql += " and Nome COLLATE Latin1_general_CI_AI LIKE '%" + filtros["Nome"].ToString() + "%' COLLATE Latin1_general_CI_AI";
                }
            }
        }

        public static int Inserir(Fornecedores s)
        {
            string sql;
            sql = "Insert into Fornecedores (Nome) Values (" + s.Nome + "')";

            return Geral.Geral.Manipular(sql);
        }

        public static int Remover(int i)
        {
            string sql;
            sql = "Delete from Fornecedores where Id = " + i.ToString();
            return Geral.Geral.Manipular(sql);
        }

        public static int AlterarDados(Fornecedores s)
        {
            string sql;
            sql = "Update Fornecedores set Nome = '" + s.Nome + "'";

            return Geral.Geral.Manipular(sql);
        }

        #endregion
        #endregion
    }
}