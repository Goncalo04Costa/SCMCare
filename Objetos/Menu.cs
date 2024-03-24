/*
*	<copyright file="menu" company="IPCA">
*	</copyright>
* 	<author>Gonçalo Costa</author>
*	<contact>a26024@alunos.ipca.pt</contact>
*   <date>2024 24/03/2024 11:09:04</date>
*	<description></description>
**/

using System;
using System.Collections.Generic;
using System.Data;

namespace Objetos
{
    public class Menu
    {
        public int Id { get; set; }
        public DateTime Dia { get; set; }
        public bool Horario { get; set; }
        public bool Tipo { get; set; }
        public int SopasId { get; set; }
        public int PratosId { get; set; }
        public int SobremesasId { get; set; }

        public Menu() { }

        public Menu(System.Data.DataRow tabela)
        {
            if (tabela.Table.Columns.Contains("Id"))
            {
                this.Id = tabela.Field<int>("Id");
            }
            if (tabela.Table.Columns.Contains("Dia"))
            {
                this.Dia = tabela.Field<DateTime>("Dia");
            }
            if (tabela.Table.Columns.Contains("Horario"))
            {
                this.Horario = tabela.Field<bool>("Horario");
            }
            if (tabela.Table.Columns.Contains("Tipo"))
            {
                this.Tipo = tabela.Field<bool>("Tipo");
            }
            if (tabela.Table.Columns.Contains("SopasId"))
            {
                this.SopasId = tabela.Field<int>("SopasId");
            }
            if (tabela.Table.Columns.Contains("PratosId"))
            {
                this.PratosId = tabela.Field<int>("PratosId");
            }
            if (tabela.Table.Columns.Contains("SobremesasId"))
            {
                this.SobremesasId = tabela.Field<int>("SobremesasId");
            }
        }

        public static Menu[] ObterLista(Dictionary<String, Object> filtros)
        {
            string sql;
            PreparaSQL(filtros, out sql);
            return new Menu[0];
        }

        private static void PreparaSQL(Dictionary<String, Object> filtros, out string sql)
        {
            sql = "SELECT * FROM Menu WHERE 1=1 ";

            if (filtros != null)
            {
                if (filtros.ContainsKey("DiaDe") && !string.IsNullOrEmpty(filtros["DiaDe"].ToString()))
                {
                    sql += " AND Dia >= '" + filtros["DiaDe"].ToString() + "'";
                }
                if (filtros.ContainsKey("DiaAte") && !string.IsNullOrEmpty(filtros["DiaAte"].ToString()))
                {
                    sql += " AND Dia <= '" + filtros["DiaAte"].ToString() + "'";
                }
            }
        }

        public static int Inserir(Menu menu)
        {
            string sql = $"INSERT INTO Menu (Dia, Horario, Tipo, SopasId, PratosId, SobremesasId) VALUES ('{menu.Dia}', '{menu.Horario}', '{menu.Tipo}', {menu.SopasId}, {menu.PratosId}, {menu.SobremesasId})";
            return 0;
        }

        public static int Remover(int id)
        {
            string sql = $"DELETE FROM Menu WHERE Id = {id}";
            return 0;
        }

        public static int AlterarDados(Menu menu)
        {
            string sql = $"UPDATE Menu SET Dia = '{menu.Dia}', Horario = '{menu.Horario}', Tipo = '{menu.Tipo}', SopasId = {menu.SopasId}, PratosId = {menu.PratosId}, SobremesasId = {menu.SobremesasId} WHERE Id = {menu.Id}";
            return 0;
        }
    }
}

