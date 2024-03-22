/*
*	<copyright file="Sobremesas" company="IPCA"></copyright>
* 	<author>Sofia Carvalho</author>
*	<contact>a25991@alunos.ipca.pt</contact>
*   <date>3/20/2024 23:02:29 PM</date>
*	<description></description>
**/

using Geral;
using System.Collections.Generic;
using System;
using System.Data;

namespace Objetos
{
    public class FornecedoresMedicamento
    {
        #region atributos
        public int MedicamentosId { get; set; }
        public int FornecedoresId { get; set; }

        #endregion
        #region Métodos 
        #region Construtores
        public FornecedoresMedicamento() { }

        /// <summary>
        /// Construtor para FornecedoresMedicamento
        /// Recebe uma tabela com dados e de acordo com as colunas vai adicionar ao objeto.
        /// </summary>
        /// <param name="tabela"> Tabela de dados. </param>
        public FornecedoresMedicamento(DataRow tabela)
        {
            if (tabela.Table.Columns.Contains("MedicamentosId"))
            {
                this.MedicamentosId = tabela.Field<int>("MedicamentosId");
            }
            if (tabela.Table.Columns.Contains("MedicamentosId"))
            {
                this.MedicamentosId = tabela.Field<int>("Id");
            }
        }

        #endregion
        #region Outros Métodos
        /// <summary>
        /// Método para obter a lista de Fornecedores e Medicamentos de acordo com o filtro.
        /// </summary>
        /// <param name="filtros">Filtro de parámetros.</param>
        /// <returns>Devolve a lista de Fornecedores e Medicamentos.</returns>
        public static FornecedoresMedicamento[] ObterLista(Dictionary<String, Object> filtros)
        {
            string sql;
            PreparaSQL(filtros, out sql);

            FornecedoresMedicamento[] lstS = Geral<FornecedoresMedicamento>.ObterLista(sql);

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
            }
        }

        public static int Inserir(FornecedoresMedicamento s)
        {
            string sql;
            sql = "Insert into FornecedoresMedicamento (MedicamentosId, FornecedoresId) Values (" + s.MedicamentosId.ToString() + ", '" + s.FornecedoresId.ToString() + "')";

            return Geral.Geral.Manipular(sql);
        }

        public static int Remover(int i, int a)
        {
            string sql;
            sql = "Delete from FornecedoresMedicamento where MedicamentosId = " + i.ToString() + " and FornecedoresId = " + a.ToString();
            return Geral.Geral.Manipular(sql);
        }

        public static int AlterarDados(FornecedoresMedicamento s)
        {
            string sql;
            sql = "Update FornecedoresMedicamento set MedicamentosId = '" + s.MedicamentosId.ToString() + "', FornecedoresId = '" + s.FornecedoresId.ToString() + "'";

            return Geral.Geral.Manipular(sql);
        }
        #endregion
        #endregion
    }
}
