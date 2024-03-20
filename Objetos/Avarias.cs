/*
*	<copyright file="Avarias" company="IPCA"></copyright>
* 	<author>Sofia Carvalho</author>
*	<contact>a25991@alunos.ipca.pt</contact>
*   <date>3/20/2024 18:34:12 PM</date>
*	<description></description>
**/

using Geral;
using System;
using System.Collections.Generic;
using System.Data;

namespace Objetos
{
    public class Avarias
    {
        #region atributos
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public int EquipamentosId { get; set; }
        public string Descricao { get; set; }
        public int Estado { get; set; }

        #endregion
        #region Métodos 
        #region Construtores
        public Avarias() { }

        /// <summary>
        /// Construtos para Avarias
        /// Recebe uma tabela com dados e de acordo com as colunas vai adicionar ao objeto.
        /// </summary>
        /// <param name="tabela"> Tabela de dados. </param>
        public Avarias(DataRow tabela)
        {
            if (tabela.Table.Columns.Contains("Id"))
            {
                this.Id = tabela.Field<int>("Id");
            }
            if (tabela.Table.Columns.Contains("Data"))
            {
                this.Data = tabela.Field<DateTime>("Data");
            }
            if (tabela.Table.Columns.Contains("EquipamentosId"))
            {
                this.EquipamentosId = tabela.Field<int>("EquipamentosId");
            }
            if (tabela.Table.Columns.Contains("Descricao"))
            {
                this.Descricao = tabela.Field<string>("Descricao");
            }
            if (tabela.Table.Columns.Contains("Estado"))
            {
                this.Estado = tabela.Field<int>("Estado");
            }
        }

        #endregion
        #region Outros Métodos
        /// <summary>
        /// Método para obter a lista de avarias de acordo com o filtro.
        /// </summary>
        /// <param name="filtros">Filtro de parámetros.</param>
        /// <returns>Devolve a lista de avarias.</returns>
        public static Avarias[] ObterLista(Dictionary<String, Object> filtros)
        {
            string sql;
            PreparaSQL(filtros, out sql);

            Avarias[] lstS = Geral<Avarias>.ObterLista(sql);

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

        public static int Inserir(Avarias s)
        {
            string sql;
            sql = "Insert into Avarias (Data, EquipamentosId, Descricao, Estado) Values (" + s.Data.ToString() + ", '" + s.EquipamentosId.ToString() + ", '" + s.Descricao + "', '" + s.Estado.ToString() + "')";

            return Geral.Geral.Manipular(sql);
        }

        public static int Remover(int i)
        {
            string sql;
            sql = "Delete from Avaliacoes where Id = " + i.ToString();
            return Geral.Geral.Manipular(sql);
        }

        public static int AlterarDados(Avarias s)
        {
            string sql;
            sql = "Update Avaliacoes set Data = '" + s.Data.ToString() + "', EquipamentosId = '" + s.EquipamentosId.ToString() + "', Descricao = '" + s.Descricao + "', Estado = '" + s.Estado.ToString() + "'";

            return Geral.Geral.Manipular(sql);
        }

        #endregion
        #endregion
    }
}
