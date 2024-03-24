/*
*	<copyright file="ContactosFornecedorescs" company="IPCA">
*	</copyright>
* 	<author>Gonçalo Costa</author>
*	<contact>a26024@alunos.ipca.pt</contact>
*   <date>2024 24/03/2024 12:06:18</date>
*	<description></description>
**/

using System;
using System.Collections.Generic;
using System.Data;
using Geral;

namespace Objetos
{
    public class ContactosFornecedores
    {
        #region Atributos

        public int FornecedoresId { get; set; }
        public int TipoContactoId { get; set; }
        public string Valor { get; set; }

        #endregion

        #region Métodos

        #region Construtores
        public ContactosFornecedores() { }

        /// <summary>
        /// Construtor para contactos de fornecedores.
        /// Recebe uma tabela com dados e de acordo com as colunas vai adicionar ao objeto.
        /// </summary>
        /// <param name="tabela">Tabela de dados.</param>
        public ContactosFornecedores(DataRow tabela)
        {
            if (tabela.Table.Columns.Contains("FornecedoresId"))
            {
                this.FornecedoresId = tabela.Field<int>("FornecedoresId");
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
        /// Método para obter a lista de contactos de fornecedores de acordo com o filtro.
        /// </summary>
        /// <param name="filtros">Filtro de parâmetros.</param>
        /// <returns>Devolve a lista de contactos de fornecedores.</returns>
        public static ContactosFornecedores[] ObterLista(Dictionary<String, Object> filtros)
        {
            string sql;
            PreparaSQL(filtros, out sql);

            ContactosFornecedores[] lstCF = Geral<ContactosFornecedores>.ObterLista(sql);

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
            sql = @"SELECT FornecedoresId, TipoContactoId, Valor FROM ContactosFornecedores WHERE 1=1 ";

            // Adicionar filtros à SQL e registar os parâmetros
            if (filtros != null)
            {
                // Adicione mais filtros conforme necessário
            }
        }

        public static int Inserir(ContactosFornecedores cf)
        {
            string sql;
            sql = "INSERT INTO ContactosFornecedores (FornecedoresId, TipoContactoId, Valor) VALUES (" + cf.FornecedoresId + ", " + cf.TipoContactoId + ", '" + cf.Valor + "')";

            return Geral.Geral.Manipular(sql);
        }


        public static int Remover(int fornecedoresId, int tipoContactoId)
        {
            string sql;
            sql = "DELETE FROM ContactosFornecedores WHERE FornecedoresId = " + fornecedoresId + " AND TipoContactoId = " + tipoContactoId;
            return Geral.Geral.Manipular(sql);
        }

        #endregion

        #endregion
    }
}
