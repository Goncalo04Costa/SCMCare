/*
*	<copyright file="Sobremesas" company="IPCA"></copyright>
* 	<author>Sofia Carvalho</author>
*	<contact>a25991@alunos.ipca.pt</contact>
*   <date>3/20/2024 20:59:47 PM</date>
*	<description></description>
**/

using MetodosGlobais;
using System;
using System.Collections.Generic;
using System.Data;

namespace ObjetosNegocio
{
    public class Consultas
    {
        #region atributos
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public string Descricao { get; set; }
        public int HospitaisId { get; set; }
        public int UtentesId { get; set; }
        public int FuncionariosId { get; set; }
        public int ResponsaveisId { get; set; }

        #endregion
        #region Métodos 
        #region Construtores
        public Consultas() { }

        /// <summary>
        /// Construtor para Avarias
        /// Recebe uma tabela com dados e de acordo com as colunas vai adicionar ao objeto.
        /// </summary>
        /// <param name="tabela"> Tabela de dados. </param>
        public Consultas(DataRow tabela)
        {
            if (tabela.Table.Columns.Contains("Id"))
            {
                this.Id = tabela.Field<int>("Id");
            }
            if (tabela.Table.Columns.Contains("Data"))
            {
                this.Data = tabela.Field<DateTime>("Data");
            }
            if (tabela.Table.Columns.Contains("Descricao"))
            {
                this.Descricao = tabela.Field<string>("Descricao");
            }
            if (tabela.Table.Columns.Contains("HospitaisId"))
            {
                this.HospitaisId = tabela.Field<int>("HospitaisId");
            }
            if (tabela.Table.Columns.Contains("UtentesId"))
            {
                this.UtentesId = tabela.Field<int>("UtentesId");
            }
            if (tabela.Table.Columns.Contains("FuncionariosId"))
            {
                this.FuncionariosId = tabela.Field<int>("FuncionariosId");
            }
            if (tabela.Table.Columns.Contains("ResponsaveisId"))
            {
                this.ResponsaveisId = tabela.Field<int>("ResponsaveisId");
            }
            
        }

        #endregion
        #region Outros Métodos

        /// <summary>
        /// Método para obter a lista de consultas de acordo com o filtro.
        /// </summary>
        /// <param name="filtros">Filtro de parámetros.</param>
        /// <returns>Devolve a lista de consultas.</returns>
        public static List<Consultas> ObterLista(Dictionary<String, Object> filtros)
        {
            string sql;
            PreparaSQL(filtros, out sql);

            List<Consultas> lstS = Geral<Consultas>.ObterLista(sql);

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

        public static int Inserir(Consultas s)
        {
            string sql;
            sql = "Insert into Consultas (Data, Descricao, HospitaisId, UtentesId, FuncionariosId, ResponsaveisId) Values (" + s.Data.ToString() + ", '" + s.Descricao + ", '" + s.HospitaisId.ToString() + "', '" + s.UtentesId.ToString() + "', '" + s.FuncionariosId.ToString() + "', '" + s.ResponsaveisId.ToString() + "')";

            return Geral.Manipular(sql);
        }

        public static int Remover(int i)
        {
            string sql;
            sql = "Delete from Consultas where Id = " + i.ToString();
            return Geral.Manipular(sql);
        }

        public static int AlterarDados(Consultas s)
        {
            string sql;
            sql = "Update Consultas set Data = '" + s.Data.ToString() + "', Descricao = '" + s.Descricao + "', HospitaisId = '" + s.HospitaisId.ToString() + "', UtentesId = '" + s.UtentesId.ToString() + "', FuncionariosId = '" + s.FuncionariosId.ToString() + "', ResponsaveisId = '" + s.ResponsaveisId.ToString() + "'";

            return Geral.Manipular(sql);
        }


        public static bool CanAttendConsulta(int responsavelId, DateTime consultaData)
        {
            if (IsAvailable(responsavelId, consultaData))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool IsAvailable(int responsavelId, DateTime consultaData)
        {
            return true; 
        }


        #endregion
        #endregion
    }
}