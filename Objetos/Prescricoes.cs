/*
*	<copyright file="Prescricoes" company="IPCA">
*	</copyright>
* 	<author>Gonçalo Costa</author>
*	<contact>a26024@alunos.ipca.pt</contact>
*   <date>2024 24/03/2024 11:48:23</date>
*	<description></description>
**/

using System;
using System.Collections.Generic;
using System.Data;
using Geral;

namespace Objetos
{
    public class Prescricoes
    {
        #region Atributos

        public int Id { get; set; }
        public int UtentesId { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public string Observacoes { get; set; }

        #endregion

        #region Métodos

        #region Construtores
        public Prescricoes() { }

        /// <summary>
        /// Construtor para prescrições.
        /// Recebe uma tabela com dados e de acordo com as colunas vai adicionar ao objeto.
        /// </summary>
        /// <param name="tabela">Tabela de dados.</param>
        public Prescricoes(DataRow tabela)
        {
            if (tabela.Table.Columns.Contains("Id"))
            {
                this.Id = tabela.Field<int>("Id");
            }
            if (tabela.Table.Columns.Contains("UtentesId"))
            {
                this.UtentesId = tabela.Field<int>("UtentesId");
            }
            if (tabela.Table.Columns.Contains("DataInicio"))
            {
                this.DataInicio = tabela.Field<DateTime>("DataInicio");
            }
            if (tabela.Table.Columns.Contains("DataFim"))
            {
                this.DataFim = tabela.Field<DateTime?>("DataFim");
            }
            if (tabela.Table.Columns.Contains("Observacoes"))
            {
                this.Observacoes = tabela.Field<string>("Observacoes");
            }
        }
        #endregion

        #region Outros Métodos

        /// <summary>
        /// Método para obter a lista de prescrições de acordo com o filtro.
        /// </summary>
        /// <param name="filtros">Filtro de parâmetros.</param>
        /// <returns>Devolve a lista de prescrições.</returns>
        public static Prescricoes[] ObterLista(Dictionary<String, Object> filtros)
        {
            string sql;
            PreparaSQL(filtros, out sql);

            Prescricoes[] lstP = Geral<Prescricoes>.ObterLista(sql);

            return lstP;
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
            sql = @"SELECT Id, UtentesId, DataInicio, DataFim, Observacoes FROM Prescricoes WHERE 1=1 ";

            // Adicionar filtros à SQL e registar os parâmetros
            if (filtros != null)
            {
                if (filtros.ContainsKey("IdDe") && !string.IsNullOrEmpty(filtros["IdDe"].ToString()))
                {
                    sql += " AND Id >= " + filtros["IdDe"].ToString();
                }
                if (filtros.ContainsKey("IdAte") && !string.IsNullOrEmpty(filtros["IdAte"].ToString()))
                {
                    sql += " AND Id <= " + filtros["IdAte"].ToString();
                }

                // Adicione mais filtros conforme necessário
            }
        }

        public static int Inserir(Prescricoes p)
        {
            string sql;
            sql = "INSERT INTO Prescricoes (UtentesId, DataInicio, DataFim, Observacoes) VALUES (" + p.UtentesId + ", '" + p.DataInicio.ToString("yyyy-MM-dd") + "', " + (p.DataFim != null ? "'" + p.DataFim.Value.ToString("yyyy-MM-dd") + "'" : "NULL") + ", '" + p.Observacoes + "')";

            return Geral.Geral.Manipular(sql);
        }


        public static int Remover(int i)
        {
            string sql;
            sql = "DELETE FROM Prescricoes WHERE Id = " + i;
            return Geral.Geral.Manipular(sql);
        }

        public static int AlterarDados(Prescricoes p)
        {
            string sql;
            sql = "UPDATE Prescricoes SET UtentesId = " + p.UtentesId + ", DataInicio = '" + p.DataInicio.ToString("yyyy-MM-dd") + "', DataFim = " + (p.DataFim != null ? "'" + p.DataFim.Value.ToString("yyyy-MM-dd") + "'" : "NULL") + ", Observacoes = '" + p.Observacoes + "' WHERE Id = " + p.Id;

            return Geral.Geral.Manipular(sql);
        }

   
        public static Prescricoes[] ObterPrescricoesPorUtente(int utenteId)
        {
            string sql = "SELECT Id, UtentesId, DataInicio, DataFim, Observacoes FROM Prescricoes WHERE UtentesId = @UtenteId";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@UtenteId", utenteId);

            return Geral<Prescricoes>.ObterLista(sql, parameters);
        }

        #endregion

        #endregion
    }
}
