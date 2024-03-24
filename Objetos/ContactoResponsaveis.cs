/*
*	<copyright file="ContactoResponsaveis" company="IPCA">
*	</copyright>
* 	<author>Gonçalo Costa</author>
*	<contact>a26024@alunos.ipca.pt</contact>
*   <date>2024 24/03/2024 12:10:55</date>
*	<description></description>
**/


using System;
using System.Collections.Generic;
using System.Data;
using Geral;

namespace Objetos
{
    public class ContactosResponsaveis
    {
        #region Atributos

        public int ResponsaveisId { get; set; }
        public int TipoContactoId { get; set; }
        public string Valor { get; set; }

        #endregion

        #region Métodos

        #region Construtores
        public ContactosResponsaveis() { }

        /// <summary>
        /// Construtor para contactos de responsáveis.
        /// Recebe uma tabela com dados e de acordo com as colunas vai adicionar ao objeto.
        /// </summary>
        /// <param name="tabela">Tabela de dados.</param>
        public ContactosResponsaveis(DataRow tabela)
        {
            if (tabela.Table.Columns.Contains("ResponsaveisId"))
            {
                this.ResponsaveisId = tabela.Field<int>("ResponsaveisId");
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
        /// Método para obter a lista de contactos de responsáveis de acordo com o filtro.
        /// </summary>
        /// <param name="filtros">Filtro de parâmetros.</param>
        /// <returns>Devolve a lista de contactos de responsáveis.</returns>
        public static ContactosResponsaveis[] ObterLista(Dictionary<String, Object> filtros)
        {
            string sql;
            PreparaSQL(filtros, out sql);

            ContactosResponsaveis[] lstCR = Geral<ContactosResponsaveis>.ObterLista(sql);

            return lstCR;
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
            sql = @"SELECT ResponsaveisId, TipoContactoId, Valor FROM ContactosResponsaveis WHERE 1=1 ";

            // Adicionar filtros à SQL e registar os parâmetros
            if (filtros != null)
            {
                // Adicione mais filtros conforme necessário
            }
        }

        public static int Inserir(ContactosResponsaveis cr)
        {
            string sql;
            sql = "INSERT INTO ContactosResponsaveis (ResponsaveisId, TipoContactoId, Valor) VALUES (" + cr.ResponsaveisId + ", " + cr.TipoContactoId + ", '" + cr.Valor + "')";

            return Geral.Geral.Manipular(sql);
        }


        public static int Remover(int responsaveisId, int tipoContactoId)
        {
            string sql;
            sql = "DELETE FROM ContactosResponsaveis WHERE ResponsaveisId = " + responsaveisId + " AND TipoContactoId = " + tipoContactoId;
            return Geral.Geral.Manipular(sql);
        }

        #endregion

        #endregion
    }
}
