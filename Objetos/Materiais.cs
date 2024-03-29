/*
*	<copyright file="Sobremesas" company="IPCA"></copyright>
* 	<author>Sofia Carvalho</author>
*	<contact>a25991@alunos.ipca.pt</contact>
*   <date>3/20/2024 23:41:51 PM</date>
*	<description></description>
**/

using MetodosGlobais;
using System.Collections.Generic;
using System;
using System.Data;
using System.Runtime.InteropServices;

namespace ObjetosNegocio
{
    public class Materiais
    {
        #region atributos
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Limite { get; set;}
        public int TiposMaterialId { get; set; }


        #endregion
        #region Métodos 
        #region Construtores
        public Materiais() { }

        /// <summary>
        /// Construtor para materiais
        /// Recebe uma tabela com dados e de acordo com as colunas vai adicionar ao objeto.
        /// </summary>
        /// <param name="tabela"> Tabela de dados. </param>
        public Materiais(DataRow tabela)
        {
            if (tabela.Table.Columns.Contains("Id"))
            {
                this.Id = tabela.Field<int>("Id");
            }
            if (tabela.Table.Columns.Contains("Nome"))
            {
                this.Nome = tabela.Field<string>("Nome");
            }
            if (tabela.Table.Columns.Contains("Limite"))
            {
                this.Limite = tabela.Field<int>("Limite");
            }
            if (tabela.Table.Columns.Contains("TiposMaterialId"))
            {
                this.TiposMaterialId = tabela.Field<int>("TiposMaterialId");
            }
        }

        #endregion
        #region Outros Métodos
        /// <summary>
        /// Método para obter a lista de materiais de acordo com o filtro.
        /// </summary>
        /// <param name="filtros">Filtro de parámetros.</param>
        /// <returns>Devolve a lista de materiais.</returns>
        public static List<Materiais> ObterLista(Dictionary<String, Object> filtros)
        {
            string sql;
            PreparaSQL(filtros, out sql);

            List<Materiais> lstS = Geral<Materiais>.ObterLista(sql);

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

        public static int Inserir(Materiais s)
        {
            string sql;
            sql = "Insert into Materiais (Nome, Limite, TiposMaterialId) Values (" + s.Nome + ", '" + s.Limite.ToString() + ", '" + s.TiposMaterialId.ToString() + "')";

            return Geral.Manipular(sql);
        }

        public static int Remover(int i)
        {
            string sql;
            sql = "Delete from Materiais where Id = " + i.ToString();
            return Geral.Manipular(sql);
        }

        public static int AlterarDados(Materiais s)
        {
            string sql;
            sql = "Update Materiais set Nome = '" + s.Nome + "', Limite = '" + s.Limite.ToString() + "', TiposMaterialId = '" + s.TiposMaterialId.ToString() + "'";

            return Geral.Manipular(sql);
        }

        /// <summary>
        /// Verifica se a quantidade atual está abaixo do limite e gera um alerta.
        /// </summary>
        public void VerificarAlertaLimite()
        {
            if (this.Limite > 0 && this.QuantidadeAtual < this.Limite)
            {
                Console.WriteLine($"Alerta: A quantidade atual de '{this.Nome}' está abaixo do limite!");
            }
        }

        #endregion
        #endregion
    }
}
