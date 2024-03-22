/*
*	<copyright file="Sobremesas" company="IPCA"></copyright>
* 	<author>Sofia Carvalho</author>
*	<contact>a25991@alunos.ipca.pt</contact>
*   <date>3/20/2024 21:14:04 PM</date>
*	<description></description>
**/

using Geral;
using System.Collections.Generic;
using System;
using System.Data;
using System.Diagnostics.Eventing.Reader;

namespace Objetos
{
    public class Equipamentos
    {
        #region atributos
        public int Id { get; set; }
        public string Descricao { get; set; }
        public bool Historico { get; set; }
        public int TiposEquipamentoId { get; set; }
        public int QuartosId { get; set; }


        #endregion
        #region Métodos 
        #region Construtores
        public Equipamentos() { }

        /// <summary>
        /// Construtor para Equipamentos
        /// Recebe uma tabela com dados e de acordo com as colunas vai adicionar ao objeto.
        /// </summary>
        /// <param name="tabela"> Tabela de dados. </param>
        public Equipamentos(DataRow tabela)
        {
            if (tabela.Table.Columns.Contains("Id"))
            {
                this.Id = tabela.Field<int>("Id");
            }
            if (tabela.Table.Columns.Contains("Descricao"))
            {
                this.Descricao = tabela.Field<string>("Descricao");
            }
            if (tabela.Table.Columns.Contains("Historico"))
            {
                this.Historico = tabela.Field<bool>("Historico");
            }
            if (tabela.Table.Columns.Contains("TiposEquipamentoId"))
            {
                this.TiposEquipamentoId = tabela.Field<int>("TiposEquipamentoId");
            }
            if (tabela.Table.Columns.Contains("QuartosId"))
            {
                this.QuartosId = tabela.Field<int>("QuartosId");
            }
        }

        #endregion
        #region Outros Métodos

        /// <summary>
        /// Método para obter a lista de equipamentos de acordo com o filtro.
        /// </summary>
        /// <param name="filtros">Filtro de parámetros.</param>
        /// <returns>Devolve a lista de equipamentos.</returns>
        public static Equipamentos[] ObterLista(Dictionary<String, Object> filtros)
        {
            string sql;
            PreparaSQL(filtros, out sql);

            Equipamentos[] lstS = Geral<Equipamentos>.ObterLista(sql);

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

        public static int Inserir(Equipamentos s)
        {
            string sql;
            sql = "Insert into Equipamentos (Descricao, Historico, TiposEquipamentoId, QuartosId) Values (" + s.Descricao + ", '" + s.Historico.ToString() + ", '" + s.TiposEquipamentoId.ToString() + "', '" + s.QuartosId.ToString() + "')";

            return Geral.Geral.Manipular(sql);
        }

        public static int Remover(int i)
        {
            string sql;
            sql = "Delete from Equipamentos where Id = " + i.ToString();
            return Geral.Geral.Manipular(sql);
        }

        public static int AlterarDados(Equipamentos s)
        {
            string sql;
            sql = "Update Equipamentos set Descricao = '" + s.Descricao + "', Historico = '" + s.Historico.ToString() + "', TiposEquipamentoId = '" + s.TiposEquipamentoId.ToString() + "', QuartosId = '" + s.QuartosId.ToString() + "'";

            return Geral.Geral.Manipular(sql);
        } 

        #endregion
        #endregion
    }
}