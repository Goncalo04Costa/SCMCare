/*
*	<copyright file="Turnos" company="IPCA">
*	</copyright>
* 	<author>Gonçalo Costa</author>
*	<contact>a26024@alunos.ipca.pt</contact>
*   <date>2024 20/03/2024 16:08:03</date>
*	<description></description>
**/

using System;
using System.Collections.Generic;
using System.Data;
using Geral;

namespace Objetos
{
    public class Turnos
    {

        #region Atributos

        public int Id { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFim { get; set; }
        public bool Ativo { get; set; }

        #endregion

        #region Métodos

        #region Construtores
        public Turnos() { }

        public Turnos(DataRow tabela)
        {
            if (tabela.Table.Columns.Contains("Id"))
            {
                this.Id = tabela.Field<int>("Id");
            }
            if (tabela.Table.Columns.Contains("HoraInicio"))
            {
                this.HoraInicio = tabela.Field<TimeSpan>("HoraInicio");
            }
            if (tabela.Table.Columns.Contains("HoraFim"))
            {
                this.HoraFim = tabela.Field<TimeSpan>("HoraFim");
            }
            if (tabela.Table.Columns.Contains("Ativo"))
            {
                this.Ativo = tabela.Field<bool>("Ativo");
            }
        }

        #region Outros Métodos

        public static Turnos[] ObterLista(Dictionary<String, Object> filtros)
        {
            string sql;
            PreparaSQL(filtros, out sql);

            Turnos[] lstT = Geral<Turnos>.ObterLista(sql);

            return lstT;
        }

        private static void PreparaSQL(Dictionary<String, Object> filtros, out string sql)
        {
            List<object> parSQL = new List<object>();
            sql = @"SELECT Id, HoraInicio, HoraFim, Ativo FROM Turnos WHERE 1=1 ";

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

                // Exemplo de como adicionar um filtro de hora de início
                if (filtros.ContainsKey("HoraInicio") && filtros["HoraInicio"] is TimeSpan)
                {
                    sql += " AND HoraInicio = '" + filtros["HoraInicio"].ToString() + "'";
                }

                // Exemplo de como adicionar um filtro de hora de fim
                if (filtros.ContainsKey("HoraFim") && filtros["HoraFim"] is TimeSpan)
                {
                    sql += " AND HoraFim = '" + filtros["HoraFim"].ToString() + "'";
                }

                // Exemplo de como adicionar um filtro para turno ativo
                if (filtros.ContainsKey("Ativo") && filtros["Ativo"] is bool)
                {
                    bool ativo = (bool)filtros["Ativo"];
                    sql += " AND Ativo = " + (ativo ? "1" : "0");
                }
            }
        }

        public static int Inserir(Turnos t)
        {
            string sql = "INSERT INTO Turnos (HoraInicio, HoraFim, Ativo) VALUES ('" + t.HoraInicio.ToString() + "', '" + t.HoraFim.ToString() + "', " + Geral.Geral.BoolToInt(t.Ativo) + ")";

            return Geral.Geral.Manipular(sql);
        }

        public static int Remover(int id)
        {
            string sql = "DELETE FROM Turnos WHERE Id = " + id.ToString();

            return Geral.Geral.Manipular(sql);
        }


        public static int AlterarDados(Turnos t)
        {
            string sql = "UPDATE Turnos SET HoraInicio = '" + t.HoraInicio.ToString() + "', HoraFim = '" + t.HoraFim.ToString() + "', Ativo = " + Geral.Geral.BoolToInt(t.Ativo) + " WHERE Id = " + t.Id.ToString();

            return Geral.Geral.Manipular(sql);
        }

        #endregion



    }
}
