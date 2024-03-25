/*
*	<copyright file="TipoPagamento" company="IPCA">
*	</copyright>
* 	<author>Gonçalo Costa</author>
*	<contact>a26024@alunos.ipca.pt</contact>
*   <date>2024 20/03/2024 16:28:59</date>
*	<description></description>
**/


using MetodosGlobais;
using System.Collections.Generic;
using System.Data;
using System;

namespace ObjetosNegocio
{
    public class TipoPagamento
    {
        #region Atributos

        public int Id { get; set; }
        public string Descricao { get; set; }

        #endregion

        #region Métodos

        #region Construtores
        public TipoPagamento() { }

        /// <summary>
        /// Construtor para tipos de pagamentos.
        /// Recebe uma tabela com dados e de acordo com as colunas vai adicionar ao objeto.
        /// </summary>
        /// <param name="tabela">Tabela de dados.</param>
        public TipoPagamento(DataRow tabela)
        {
            if (tabela.Table.Columns.Contains("Id"))
            {
                this.Id = tabela.Field<int>("Id");
            }
            if (tabela.Table.Columns.Contains("Descricao"))
            {
                this.Descricao = tabela.Field<string>("Descricao");
            }
        }
        #endregion

        #region Outros Métodos

        /// <summary>
        /// Método para obter a lista de tipos de pagamentos.
        /// </summary>
        /// <returns>Devolve a lista de tipos de pagamentos.</returns>
        public static List<TipoPagamento> ObterLista()
        {
            string sql = "SELECT Id, Descricao FROM TipoPagamentos";
            List<TipoPagamento> listaTiposPagamento = Geral<TipoPagamento>.ObterLista(sql);
            return listaTiposPagamento;
        }

        /// <summary>
        /// Método para preparar a query SQL com os filtros obtidos.
        /// </summary>
        /// <param name="filtros">Filtros a aplicar.</param>
        /// <param name="sql">Query SQL.</param>
        private static void PreparaSQL(Dictionary<String, Object> filtros, out string sql)
        {
            // Parâmetros a devolver no final
            List<object> parSQL = new List<object>();
            sql = @"SELECT Id, Descricao FROM TipoPagamentos WHERE 1=1 ";

            // Adicionar filtros ao SQL, e registrar os parâmetros
            if (filtros != null)
            {
                // Para int - Aplica filtro para um intervalo de Ids.
                if (filtros.ContainsKey("IdDe") && !string.IsNullOrEmpty(filtros["IdDe"].ToString()))
                {
                    sql += " AND Id >= " + filtros["IdDe"].ToString();
                }
                if (filtros.ContainsKey("IdAte") && !string.IsNullOrEmpty(filtros["IdAte"].ToString()))
                {
                    sql += " AND Id <= " + filtros["IdAte"].ToString();
                }

                // Para string - Verifica se existe alguma string como a recebida no filtro (ignorando a capitalização e acentuação)
                if (filtros.ContainsKey("Descricao") && !string.IsNullOrEmpty(filtros["Descricao"].ToString()))
                {
                    sql += " AND Descricao COLLATE Latin1_general_CI_AI LIKE '%" + filtros["Descricao"].ToString() + "%' COLLATE Latin1_general_CI_AI";
                }
            }
        }
        public static int Inserir(TipoPagamento tipoPagamento)
        {
            string sql = "INSERT INTO TipoPagamentos (Descricao) VALUES ('" + tipoPagamento.Descricao + "')";
            return Geral.Manipular(sql);
        }

        public static int Remover(int id)
        {
            string sql = "DELETE FROM TipoPagamentos WHERE Id = " + id;
            return Geral.Manipular(sql);
        }

        public static int AlterarDados(TipoPagamento tipoPagamento)
        {
            string sql = "UPDATE TipoPagamentos SET Descricao = '" + tipoPagamento.Descricao + "' WHERE Id = " + tipoPagamento.Id;
            return Geral.Manipular(sql);
        }
        #endregion

        #endregion
    }
}