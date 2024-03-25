/*
*	<copyright file="Senhas" company="IPCA">
*	</copyright>
* 	<author>Gonçalo Costa</author>
*	<contact>a26024@alunos.ipca.pt</contact>
*   <date>2024 24/03/2024 11:54:45</date>
*	<description></description>
**/


using System;
using System.Collections.Generic;
using System.Data;
using MetodosGlobais;

namespace ObjetosNegocio
{
    public class Senhas
    {
        #region Atributos

        public int FuncionariosId { get; set; }
        public int MenuId { get; set; }
        public int Estado { get; set; }

        #endregion

        #region Métodos

        #region Construtores
        public Senhas() { }

        /// <summary>
        /// Construtor para senhas.
        /// Recebe uma tabela com dados e de acordo com as colunas vai adicionar ao objeto.
        /// </summary>
        /// <param name="tabela">Tabela de dados.</param>
        public Senhas(DataRow tabela)
        {
            if (tabela.Table.Columns.Contains("FuncionariosId"))
            {
                this.FuncionariosId = tabela.Field<int>("FuncionariosId");
            }
            if (tabela.Table.Columns.Contains("MenuId"))
            {
                this.MenuId = tabela.Field<int>("MenuId");
            }
            if (tabela.Table.Columns.Contains("Estado"))
            {
                this.Estado = tabela.Field<int>("Estado");
            }
        }
        #endregion

        #region Outros Métodos

        /// <summary>
        /// Método para obter a lista de senhas de acordo com o filtro.
        /// </summary>
        /// <param name="filtros">Filtro de parâmetros.</param>
        /// <returns>Devolve a lista de senhas.</returns>
        public static List<Senhas> ObterLista(Dictionary<String, Object> filtros)
        {
            string sql;
            PreparaSQL(filtros, out sql);

            List<Senhas> lstS = Geral<Senhas>.ObterLista(sql);

            return lstS;
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
            sql = @"SELECT FuncionariosId, MenuId, Estado FROM Senhas WHERE 1=1 ";

            // Adicionar filtros à SQL e registar os parâmetros
            if (filtros != null)
            {
                // Adicione mais filtros conforme necessário
            }
        }

        public static int Inserir(Senhas s)
        {
            string sql;
            sql = "INSERT INTO Senhas (FuncionariosId, MenuId, Estado) VALUES (" + s.FuncionariosId + ", " + s.MenuId + ", " + s.Estado + ")";

            return Geral.Manipular(sql);
        }


        public static int Remover(int funcId, int menuId)
        {
            string sql;
            sql = "DELETE FROM Senhas WHERE FuncionariosId = " + funcId + " AND MenuId = " + menuId;
            return Geral.Manipular(sql);
        }

        public static int AlterarEstado(int funcId, int menuId, int novoEstado)
        {
            string sql;
            sql = "UPDATE Senhas SET Estado = " + novoEstado + " WHERE FuncionariosId = " + funcId + " AND MenuId = " + menuId;

            return Geral.Manipular(sql);
        }

        #endregion

        #endregion
    }
}
