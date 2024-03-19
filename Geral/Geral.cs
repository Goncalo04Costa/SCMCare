﻿/*
*	<copyright file="Geral" company="IPCA"></copyright>
* 	<author>Diogo Fernandes</author>
*	<contact>a26008@alunos.ipca.pt</contact>
*   <date>3/19/2024 4:38:39 PM</date>
*	<description></description>
**/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Geral
{
    public class Geral<TipoDados>
    {
        #region Base de Dados

        /// <summary>
        /// Método para obter os dados de uma lista de acordo com a query sql recebida.
        /// </summary>
        /// <param name="sql">Query SQL a ser executada.</param>
        /// <returns>Devolve uma lista do tipo de dados pretendido.</returns>
        /// <exception cref="Exception"> Excessão para quando ocorre um erro.</exception>
        public static TipoDados[] ObterLista(string sql)
        {
            DataTable resultado = new DataTable(); 
            string connectionString = "Data Source=DESKTOP-BAJ0CE4;Initial Catalog=PDS;User ID=DESKTOP-BAJ0CE4\\diogo;Integrated Security=True;";

            using (SqlConnection ligacao = new SqlConnection(connectionString))
            {
                try
                {
                    ligacao.Open();

                    SqlCommand comando = new SqlCommand(sql, ligacao);
                    SqlDataAdapter adaptador = new SqlDataAdapter(comando);

                    adaptador.Fill(resultado);

                    ligacao.Close();
                    adaptador.Dispose();
                }
                catch (Exception ex)
                {
                    throw new Exception("ERRO: ", ex);
                }
            }

            return Converter(resultado);
        }

        /// <summary>
        /// Método para converter a DataTable numa lista.
        /// </summary>
        /// <param name="tabela">DataTable a converter</param>
        /// <returns>Devolve uma lista do tipo de dados pretendido.</returns>
        /// <exception cref="Exception"> Excessão para quando ocorre um erro.</exception>
        private static TipoDados[] Converter(DataTable tabela)
        {
            try
            {
                List<TipoDados> lstConvertidos = new List<TipoDados>();

                foreach (DataRow item in tabela.Rows)
                {
                    TipoDados aux = (TipoDados)Activator.CreateInstance(typeof(TipoDados), item);
                    lstConvertidos.Add(aux);
                }

                return lstConvertidos.ToArray();
            }
            catch (Exception ex)
            {
                throw new Exception("ERRO: ", ex);
            }
        }

        #endregion
    }
}
