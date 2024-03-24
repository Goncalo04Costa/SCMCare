/*
*	<copyright file="ContactosFuncionarioscs" company="IPCA">
*	</copyright>
* 	<author>Gonçalo Costa</author>
*	<contact>a26024@alunos.ipca.pt</contact>
*   <date>2024 24/03/2024 12:06:33</date>
*	<description></description>
**/

using System;
using System.Collections.Generic;
using System.Data;
using Geral;

namespace Objetos
{
    public class ContactosFuncionarios
    {
        #region Atributos

        public int FuncionariosId { get; set; }
        public int TipoContactoId { get; set; }
        public string Valor { get; set; }

        #endregion

        #region Métodos

        #region Construtores
        public ContactosFuncionarios() { }

        /// <summary>
        /// Construtor para contactos de funcionários.
        /// Recebe uma tabela com dados e de acordo com as colunas vai adicionar ao objeto.
        /// </summary>
        /// <param name="tabela">Tabela de dados.</param>
        public ContactosFuncionarios(DataRow tabela)
        {
            if (tabela.Table.Columns.Contains("FuncionariosId"))
            {
                this.FuncionariosId = tabela.Field<int>("FuncionariosId");
            }
            if (tabela.Table.Columns.Contains("TipoContactoId"))
            {
                this.TipoContactoId = tabela.Field<int>("TipoContactoId");
            }
            if (tabela.Table.Columns.Contains("Valor"))
            {
                this.Valor = tabela.Field<string>("Valor");
            }
        }
        #endregion

        #region Outros Métodos

        /// <summary>
        /// Método para obter a lista de contactos de funcionários de acordo com o filtro.
        /// </summary>
        /// <param name="filtros">Filtro de parâmetros.</param>
        /// <returns>Devolve a lista de contactos de funcionários.</returns>
        public static ContactosFuncionarios[] ObterLista(Dictionary<String, Object> filtros)
        {
            string sql;
            PreparaSQL(filtros, out sql);

            ContactosFuncionarios[] lstCF = Geral<ContactosFuncionarios>.ObterLista(sql);

            return lstCF;
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
            sql = @"SELECT FuncionariosId, TipoContactoId, Valor FROM ContactosFuncionarios WHERE 1=1 ";

            // Adicionar filtros à SQL e registar os parâmetros
            if (filtros != null)
            {
                // Adicione mais filtros conforme necessário
            }
        }

        public static int Inserir(ContactosFuncionarios cf)
        {
            string sql;
            sql = "INSERT INTO ContactosFuncionarios (FuncionariosId, TipoContactoId, Valor) VALUES (" + cf.FuncionariosId + ", " + cf.TipoContactoId + ", '" + cf.Valor + "')";

            return Geral.Geral.Manipular(sql);
        }


        public static int Remover(int funcionariosId, int tipoContactoId)
        {
            string sql;
            sql = "DELETE FROM ContactosFuncionarios WHERE FuncionariosId = " + funcionariosId + " AND TipoContactoId = " + tipoContactoId;
            return Geral.Geral.Manipular(sql);
        }

        #endregion

        #endregion
    }
}
