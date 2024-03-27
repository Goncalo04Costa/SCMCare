/*
*	<copyright file="Medicamentos" company="IPCA">
*	</copyright>
* 	<author>Gonçalo Costa</author>
*	<contact>a26024@alunos.ipca.pt</contact>
*   <date>2024 24/03/2024 12:03:02</date>
*	<description></description>
**/

using System;
using System.Collections.Generic;
using System.Data;
using MetodosGlobais;

namespace ObjetosNegocio
{
    public class Medicamentos
    {
        #region Atributos

        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int Limite { get; set; }

        #endregion

        #region Métodos

        #region Construtores
        public Medicamentos() { }

        /// <summary>
        /// Construtor para medicamentos.
        /// Recebe uma tabela com dados e de acordo com as colunas vai adicionar ao objeto.
        /// </summary>
        /// <param name="tabela">Tabela de dados.</param>
        public Medicamentos(DataRow tabela)
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
            if (tabela.Table.Columns.Contains("Limite"))
            {
                this.Limite = tabela.Field<int>("Limite");
            }
        }
        #endregion

        #region Outros Métodos

        /// <summary>
        /// Método para obter a lista de medicamentos de acordo com o filtro.
        /// </summary>
        /// <param name="filtros">Filtro de parâmetros.</param>
        /// <returns>Devolve a lista de medicamentos.</returns>
        public static List<Medicamentos> ObterLista(Dictionary<String, Object> filtros)
        {
            string sql;
            PreparaSQL(filtros, out sql);

            List<Medicamentos> lstM = Geral<Medicamentos>.ObterLista(sql);

            return lstM;
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
            sql = @"SELECT Id, Nome, Descricao, Limite FROM Medicamentos WHERE 1=1 ";

            // Adicionar filtros à SQL e registar os parâmetros
            if (filtros != null)
            {
                // Adicione mais filtros conforme necessário
            }
        }

        public static int Inserir(Medicamentos m)
        {
            string sql;
            sql = "INSERT INTO Medicamentos (Nome, Descricao, Limite) VALUES ('" + m.Nome + "', '" + m.Descricao + "', " + m.Limite + ")";

            return Geral.Manipular(sql);
        }


        public static int Remover(int i)
        {
            string sql;
            sql = "DELETE FROM Medicamentos WHERE Id = " + i;
            return Geral.Manipular(sql);
        }

        public static int AlterarDados(Medicamentos m)
        {
            string sql;
            sql = "UPDATE Medicamentos SET Nome = '" + m.Nome + "', Descricao = '" + m.Descricao + "', Limite = " + m.Limite + " WHERE Id = " + m.Id;

            return Geral.Manipular(sql);
        }


   
        public void VerificarAlertaLimite()
        {
            if (this.Limite > 0 && this.QuantidadeAtual < this.Limite)
            {
                Console.WriteLine($"Alerta: A quantidade atual de '{this.Nome}' está abaixo do limite!");
               
            }
        }

        #endregion

        #endregion
    }
}
