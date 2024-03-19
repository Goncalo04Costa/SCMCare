﻿/*
*	<copyright file="Sobremesas" company="IPCA"></copyright>
* 	<author>Diogo Fernandes</author>
*	<contact>a26008@alunos.ipca.pt</contact>
*   <date>3/19/2024 3:43:30 PM</date>
*	<description></description>
**/

using System;
using System.Collections.Generic;
using System.Data;
using Geral;

namespace Objetos
{
    public class Sobremesas
    {
        #region Atributos

        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public bool Tipo { get; set; }

        #endregion

        #region Métodos

        #region Construtores
        public Sobremesas() { }

        /// <summary>
        /// Construtor para sobremesa.
        /// Recebe uma tabela com dados e de acordo com as colunas vai adicionar ao objeto.
        /// </summary>
        /// <param name="tabela">Tabela de dados.</param>
        public Sobremesas(DataRow tabela)
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
            if (tabela.Table.Columns.Contains("Tipo"))
            {
                this.Tipo = tabela.Field<bool>("Tipo");
            }
        }
        #endregion

        #region Outros Métodos

        public static Sobremesas[] ObterLista(Dictionary<String, Object> filtros)
        {
            string sql;
            PreparaSQL(filtros, out sql);
            
            Sobremesas[] lstS = Geral<Sobremesas>.ObterLista(sql);

            return lstS;
        }

        private static void PreparaSQL(Dictionary<String, Object> filtros, out string sql)
        {
            // Parámetros a devolver no final
            List<object> parSQL = new List<object>();
            sql = @"Select Id, Nome, Descricao, Tipo From Sobremesas where 1=1 ";


            // Adicionar filtros ao sql, e registar os parámetros
            if (filtros != null)
            {
                if (filtros.ContainsKey("IdDe") && !string.IsNullOrEmpty(filtros["IdDe"].ToString()))
                {
                    sql += " and Id >= " + filtros["IdDe"].ToString();
                }
                if (filtros.ContainsKey("IdAte") && !string.IsNullOrEmpty(filtros["IdAte"].ToString()))
                {
                    sql += " and Id <= @" + filtros["IdAte"].ToString();
                }
                //  !!! Verificar como funciona antes de implementar
                //if (filtros.ContainsKey("NomeDe") && !string.IsNullOrEmpty(filtros["filtros"].ToString()))
                //{
                //    sql += " and c.Nome COLLATE Latin1_general_CI_AI LIKE " + filtros["NomeDe"].ToString() + " COLLATE Latin1_general_CI_AI";
                //}

                // Para bit/bool - Verifica se tem filtro para True, verifica se tem filtro para False, se já tiver filtro para True, adiciona 'or',
                // se a string não estiver vazia adiciona os filtros à string SQL.
                String Tipo = "";
                if (filtros.ContainsKey("Tipo1") && filtros["Tipo1"].ToString() == "1")
                {
                    Tipo += "Tipo=1";
                }
                if (filtros.ContainsKey("Tipo0") && filtros["Tipo0"].ToString() == "1")
                {
                    if (!String.IsNullOrWhiteSpace(Tipo))
                        Tipo += " or ";
                    Tipo += "Tipo=0";
                }
                if (!String.IsNullOrWhiteSpace(Tipo))
                    sql += String.Format(" and ({0})", Tipo);

            }
        }

        #endregion

        #endregion
    }
}
