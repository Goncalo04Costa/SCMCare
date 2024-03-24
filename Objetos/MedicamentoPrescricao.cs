/*
*	<copyright file="MedicamentoPrescricao" company="IPCA">
*	</copyright>
* 	<author>Gonçalo Costa</author>
*	<contact>a26024@alunos.ipca.pt</contact>
*   <date>2024 24/03/2024 12:00:51</date>
*	<description></description>
**/

using System;
using System.Collections.Generic;
using System.Data;
using Geral;

namespace Objetos
{
    public class MedicamentosPrescricao
    {
        #region Atributos

        public int PrescricoesId { get; set; }
        public int MedicamentosId { get; set; }
        public int QuantidadePIntervalo { get; set; }
        public int IntervaloHoras { get; set; }
        public string Instrucoes { get; set; }

        #endregion

        #region Métodos

        #region Construtores
        public MedicamentosPrescricao() { }

        /// <summary>
        /// Construtor para MedicamentosPrescricao.
        /// Recebe uma tabela com dados e de acordo com as colunas vai adicionar ao objeto.
        /// </summary>
        /// <param name="tabela">Tabela de dados.</param>
        public MedicamentosPrescricao(DataRow tabela)
        {
            if (tabela.Table.Columns.Contains("PrescricoesId"))
            {
                this.PrescricoesId = tabela.Field<int>("PrescricoesId");
            }
            if (tabela.Table.Columns.Contains("MedicamentosId"))
            {
                this.MedicamentosId = tabela.Field<int>("MedicamentosId");
            }
            if (tabela.Table.Columns.Contains("QuantidadePIntervalo"))
            {
                this.QuantidadePIntervalo = tabela.Field<int>("QuantidadePIntervalo");
            }
            if (tabela.Table.Columns.Contains("IntervaloHoras"))
            {
                this.IntervaloHoras = tabela.Field<int>("IntervaloHoras");
            }
            if (tabela.Table.Columns.Contains("Instrucoes"))
            {
                this.Instrucoes = tabela.Field<string>("Instrucoes");
            }
        }
        #endregion

        #region Outros Métodos

        /// <summary>
        /// Método para obter a lista de MedicamentosPrescricao de acordo com o filtro.
        /// </summary>
        /// <param name="filtros">Filtro de parâmetros.</param>
        /// <returns>Devolve a lista de MedicamentosPrescricao.</returns>
        public static MedicamentosPrescricao[] ObterLista(Dictionary<String, Object> filtros)
        {
            string sql;
            PreparaSQL(filtros, out sql);

            MedicamentosPrescricao[] lstMP = Geral<MedicamentosPrescricao>.ObterLista(sql);

            return lstMP;
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
            sql = @"SELECT PrescricoesId, MedicamentosId, QuantidadePIntervalo, IntervaloHoras, Instrucoes FROM MedicamentosPrescricao WHERE 1=1 ";

            // Adicionar filtros à SQL e registar os parâmetros
            if (filtros != null)
            {
                // Adicione mais filtros conforme necessário
            }
        }

        public static int Inserir(MedicamentosPrescricao mp)
        {
            string sql;
            sql = "INSERT INTO MedicamentosPrescricao (PrescricoesId, MedicamentosId, QuantidadePIntervalo, IntervaloHoras, Instrucoes) VALUES (" + mp.PrescricoesId + ", " + mp.MedicamentosId + ", " + mp.QuantidadePIntervalo + ", " + mp.IntervaloHoras + ", '" + mp.Instrucoes + "')";

            return Geral.Geral.Manipular(sql);
        }


        public static int Remover(int prescricoesId, int medicamentosId)
        {
            string sql;
            sql = "DELETE FROM MedicamentosPrescricao WHERE PrescricoesId = " + prescricoesId + " AND MedicamentosId = " + medicamentosId;
            return Geral.Geral.Manipular(sql);
        }

        #endregion

        #endregion
    }
}
