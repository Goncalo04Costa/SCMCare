/*
*	<copyright file="TipoEquipamento" company="IPCA">
*	</copyright>
* 	<author>Gonçalo Costa</author>
*	<contact>a26024@alunos.ipca.pt</contact>
*   <date>2024 20/03/2024 16:32:08</date>
*	<description></description>
**/

using MetodosGlobais;
using System.Collections.Generic;
using System.Data;
using System;

namespace ObjetosNegocio
{
    public class TipoEquipamento
    {
        #region Atributos

        public int Id { get; set; }
        public string Descricao { get; set; }

        #endregion

        #region Métodos

        #region Construtores
        public TipoEquipamento() { }

        /// <summary>
        /// Construtor para tipos de equipamentos.
        /// Recebe uma tabela com dados e de acordo com as colunas vai adicionar ao objeto.
        /// </summary>
        /// <param name="tabela">Tabela de dados.</param>
        public TipoEquipamento(DataRow tabela)
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
        /// Método para obter a lista de tipos de equipamentos.
        /// </summary>
        /// <returns>Devolve a lista de tipos de equipamentos.</returns>
        public static List<TipoEquipamento> ObterLista()
        {
            string sql = "SELECT Id, Descricao FROM TipoEquipamentos";
            List<TipoEquipamento> listaTiposEquipamento = Geral<TipoEquipamento>.ObterLista(sql);
            return listaTiposEquipamento;
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
            sql = @"SELECT Id, Descricao FROM TipoEquipamentos WHERE 1=1 ";

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
        public static int Inserir(TipoEquipamento tipoEquipamento)
        {
            string sql = "INSERT INTO TipoEquipamentos (Descricao) VALUES ('" + tipoEquipamento.Descricao + "')";
            return Geral.Manipular(sql);
        }

        public static int Remover(int id)
        {
            string sql = "DELETE FROM TipoEquipamentos WHERE Id = " + id;
            return Geral.Manipular(sql);
        }

        public static int AlterarDados(TipoEquipamento tipoEquipamento)
        {
            string sql = "UPDATE TipoEquipamentos SET Descricao = '" + tipoEquipamento.Descricao + "' WHERE Id = " + tipoEquipamento.Id;
            return Geral.Manipular(sql);
        }
        #endregion

        #endregion
    }
}