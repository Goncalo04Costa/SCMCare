/*
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
    public class Geral<TDataType>
    {
        #region Base de Dados

        public static TDataType[] ObterLista(string sql)
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

        // !!! Verificar como Funciona
        private static TDataType[] Converter(DataTable tabela)
        {
            try
            {
                List<TDataType> lstConvertidos = new List<TDataType>();

                foreach (DataRow item in tabela.Rows)
                {
                    TDataType aux = (TDataType)Activator.CreateInstance(typeof(TDataType), item);
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
