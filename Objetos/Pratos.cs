/*
*	<copyright file="Pratos" company="IPCA">
*	</copyright>
* 	<author>Gonçalo Costa</author>
*	<contact>a26024@alunos.ipca.pt</contact>
*   <date>2024 19/03/2024 22:29:16</date>
*	<description></description>
**/

using System;
using System.Collections.Generic;
using System.Data;
using MetodosGlobais;

namespace ObjetosNegocio
{
    public class Pratos
    {
        #region Atributos

        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public bool Tipo { get; set; }

        #endregion

        #region Métodos

        #region Construtores
        public Pratos() { }

        public Pratos(DataRow tabela)
        {
            if (tabela.Table.Columns.Contains("Id"))
            {
                this.Id = tabela.Field<int>("Id");
            }
            if (tabela.Table.Columns.Contains("Nome"))
            {
                this.Nome = tabela.Field<string>("Nome");
            }
            if (tabela.Table.Columns.Contains("Descricao"))
            {
                this.Descricao = tabela.Field<string>("Descricao");
            }
            if (tabela.Table.Columns.Contains("Tipo"))
            {
                this.Tipo = tabela.Field<bool>("Tipo");
            }
        }
        #endregion

        #region Outros Métodos
        public static List<Pratos> ObterLista(Dictionary<String, Object> filtros)
        {
            string sql;
            PreparaSQL(filtros, out sql);
            List<Pratos> lstP = Geral<Pratos>.ObterLista(sql);

            return lstP;
        }
        /// <summary>
        /// Método para preparar a query sql com os filtros obtidos.
        /// </summary>
        /// <param name="filtros">Filtros a aplicar.</param>
        /// <param name="sql">Query sql.</param>

        private static void PreparaSQL(Dictionary<String, Object> filtros, out string sql)
            // Parámetros a devolver no final
        {
            List<object> parSQL = new List<object>();
            sql = @"Select Id, Nome, Descricao, Tipo From Pratos where 1=1 ";

            // Adicionar filtros ao sql, e registar os parámetros

            if (filtros != null)
                // Para int - Aplica filtro para um intervalo de Ids.
            {
                if (filtros.ContainsKey("IdDe") && !string.IsNullOrEmpty(filtros["IdDe"].ToString()))
                {
                    sql += " and Id >= " + filtros["IdDe"].ToString();
                }
                if (filtros.ContainsKey("IdAte") && !string.IsNullOrEmpty(filtros["IdAte"].ToString()))
                {
                    sql += " and Id <= @" + filtros["IdAte"].ToString();
                }

                if (filtros.ContainsKey("Nome") && !string.IsNullOrEmpty(filtros["Nome"].ToString()))
                {
                    sql += " and Nome COLLATE Latin1_general_CI_AI LIKE '%" + filtros["Nome"].ToString() + "%' COLLATE Latin1_general_CI_AI";
                }
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

        public static int Inserir(Pratos p)
        {
            string sql;
            sql = "Insert into Pratos (Nome, Descricao, Tipo) Values ('" + p.Nome.ToString() + "', '" + p.Descricao.ToString() + "', " + Geral.BoolToInt(p.Tipo) + ")";

            return Geral.Manipular(sql);
        }


        public static int Remover(int i)
        {
            string sql;
            sql = "Delete from Pratos where id = " + i.ToString();
            return Geral.Manipular(sql);
        }

        public static int AlterarDados(Pratos p)
        {
            string sql = "Update Pratos set Nome ='" + p.Nome.ToString() + "' ,Descricao ='" + p.Descricao.ToString() + "' ,Tipo =" + Geral.BoolToInt(p.Tipo) + ")";
            
        return Geral.Manipular(sql);
        }

        #endregion

        #endregion
    }
}