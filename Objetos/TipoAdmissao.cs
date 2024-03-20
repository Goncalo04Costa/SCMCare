/*
*	<copyright file="TipoAdmissao" company="IPCA">
*	</copyright>
* 	<author>Gonçalo Costa</author>
*	<contact>a26024@alunos.ipca.pt</contact>
*   <date>2024 20/03/2024 16:35:23</date>
*	<description></description>
**/


using Geral;
using System.Collections.Generic;
using System.Data;
using System;

namespace Objetos
{
    public class TipoAdmissao
    {
        #region Atributos

        public int Id { get; set; }
        public string Descricao { get; set; }

        #endregion

        #region Métodos

        #region Construtores
        public TipoAdmissao() { }

        /// <summary>
        /// Construtor para tipos de admissão.
        /// Recebe uma tabela com dados e de acordo com as colunas vai adicionar ao objeto.
        /// </summary>
        /// <param name="tabela">Tabela de dados.</param>
        public TipoAdmissao(DataRow tabela)
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
        /// Método para obter a lista de tipos de admissão.
        /// </summary>
        /// <returns>Devolve a lista de tipos de admissão.</returns>
        public static TipoAdmissao[] ObterLista()
        {
            string sql = "SELECT Id, Descricao FROM TipoAdmissoes";
            TipoAdmissao[] listaTiposAdmissao = Geral<TipoAdmissao>.ObterLista(sql);
            return listaTiposAdmissao;
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
            sql = @"SELECT Id, Descricao FROM TipoAdmissoes WHERE 1=1 ";

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
        public static int Inserir(TipoAdmissao tipoAdmissao)
        {
            string sql = "INSERT INTO TipoAdmissoes (Descricao) VALUES ('" + tipoAdmissao.Descricao + "')";
            return Geral.Geral.Manipular(sql);
        }

        public static int Remover(int id)
        {
            string sql = "DELETE FROM TipoAdmissoes WHERE Id = " + id;
            return Geral.Geral.Manipular(sql);
        }

        public static int AlterarDados(TipoAdmissao tipoAdmissao)
        {
            string sql = "UPDATE TipoAdmissoes SET Descricao = '" + tipoAdmissao.Descricao + "' WHERE Id = " + tipoAdmissao.Id;
            return Geral.Geral.Manipular(sql);
        }
        #endregion

        #endregion
    }
}
