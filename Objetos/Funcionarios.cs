/*
*	<copyright file="Sobremesas" company="IPCA"></copyright>
* 	<author>Sofia Carvalho</author>
*	<contact>a25991@alunos.ipca.pt</contact>
*   <date>3/20/2024 23:10:29 PM</date>
*	<description></description>
**/

using Geral;
using System.Collections.Generic;
using System;
using System.Data;

namespace Objetos
{
    public class Funcionarios
    {
        #region atributos
        public int Id { get; set; } 
        public string Nome { get; set; }
        public int TiposFuncionarioId { get; set; }
        public bool Historico { get; set; }

        #endregion
        #region Métodos 
        #region Construtores
        public Funcionarios() { }

        /// <summary>
        /// Construtor para Funcionarios
        /// Recebe uma tabela com dados e de acordo com as colunas vai adicionar ao objeto.
        /// </summary>
        /// <param name="tabela"> Tabela de dados. </param>
        public Funcionarios(DataRow tabela)
        {
            if (tabela.Table.Columns.Contains("Id"))
            {
                this.Id = tabela.Field<int>("Id");
            }
            if (tabela.Table.Columns.Contains("Nome"))
            {
                this.Nome = tabela.Field<string>("Nome");
            }
            if (tabela.Table.Columns.Contains("TiposFuncionarioId"))
            {
                this.TiposFuncionarioId = tabela.Field<int>("TiposFuncionarioId");
            }
            if (tabela.Table.Columns.Contains("Historico"))
            {
                this.Historico = tabela.Field<bool>("Historico");
            }
        }

        #endregion
        #region Outros Métodos
        /// <summary>
        /// Método para obter a lista de funcionarios de acordo com o filtro.
        /// </summary>
        /// <param name="filtros">Filtro de parámetros.</param>
        /// <returns>Devolve a lista de funcionarios.</returns>
        public static Funcionarios[] ObterLista(Dictionary<String, Object> filtros)
        {
            string sql;
            PreparaSQL(filtros, out sql);

            Funcionarios[] lstS = Geral<Funcionarios>.ObterLista(sql);

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

                // Para bit/bool - Verifica se tem filtro para True, verifica se tem filtro para False, se já tiver filtro para True, adiciona 'or',
                // se a string não estiver vazia adiciona os filtros à string SQL.
                String Tipo = "";
                if (filtros.ContainsKey("Tipo1") && filtros["Tipo1"].ToString() == "1")
                {
                    Tipo += "Tipo=1";
                }
                if (filtros.ContainsKey("Tipo0") && filtros["Tipo0"].ToString() == "1")
                {
                    if (!String.IsNullOrWhiteSpace(Tipo))
                        Tipo += " or ";
                    Tipo += "Tipo=0";
                }
                if (!String.IsNullOrWhiteSpace(Tipo))
                    sql += String.Format(" and ({0})", Tipo);
            }
        }

        public static int Inserir(Funcionarios s)
        {
            string sql;
            sql = "Insert into Funcionarios (Nome, TiposFuncionarioId, Historico) Values (" + s.Nome + ", '" + s.TiposFuncionarioId.ToString() + ", '" + s.Historico.ToString() + "')";

            return Geral.Geral.Manipular(sql);
        }

        public static int Remover(int i)
        {
            string sql;
            sql = "Delete from Funcionarios where Id = " + i.ToString();
            return Geral.Geral.Manipular(sql);
        }

        public static int AlterarDados(Funcionarios s)
        {
            string sql;
            sql = "Update Funcionarios set Nome = '" + s.Nome + "', TiposFuncionarioId = '" + s.TiposFuncionarioId.ToString() + "', Historico = '" + s.Historico.ToString() + "'";

            return Geral.Geral.Manipular(sql);
        }
        #endregion
        #endregion
    }
}