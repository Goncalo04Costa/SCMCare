/*
*	<copyright file="Utentes" company="IPCA">
*	</copyright>
* 	<author>Gonçalo Costa</author>
*	<contact>a26024@alunos.ipca.pt</contact>
*   <date>2024 20/03/2024 15:46:42</date>
*	<description></description>
**/


using System;
using System.Collections.Generic;
using System.Data;
using MetodosGlobais;

namespace ObjetosNegocio
{
    public class Utentes
    {
        #region Atributos

        public int Id { get; set; }
        public string Nome { get; set; }
        public int NIF { get; set; }
        public int SNS { get; set; }
        public DateTime DataAdmissao { get; set; }
        public DateTime DataNascimento { get; set; }
        public bool Historico { get; set; }
        public bool Tipo { get; set; }
        public int TiposAdmissaoId { get; set; }
        public string TipoAdmissao { get; set; }
        public string MotivoAdmissao { get; set; }
        public string DiagnosticoAdmissao { get; set; }
        public string Observacoes { get; set; }
        public string NotaAdmissao { get; set; }
        public string AntecedentesPessoais { get; set; }
        public string ExameObjetivo { get; set; }
        public double Mensalidade { get; set; }
        public double Cofinanciamento { get; set; }

        #endregion

        #region Métodos

        #region Construtores
        public Utentes() { }

        /// <summary>
        /// Construtor para utentes.
        /// Recebe uma tabela com dados e de acordo com as colunas vai adicionar ao objeto.
        /// </summary>
        /// <param name="tabela">Tabela de dados.</param>
        public Utentes(DataRow tabela)
        {
            if (tabela.Table.Columns.Contains("Id"))
            {
                this.Id = tabela.Field<int>("Id");
            }
            if (tabela.Table.Columns.Contains("Nome"))
            {
                this.Nome = tabela.Field<string>("Nome");
            }
            if (tabela.Table.Columns.Contains("NIF"))
            {
                this.NIF = tabela.Field<int>("NIF");
            }
            if (tabela.Table.Columns.Contains("SNS"))
            {
                this.SNS = tabela.Field<int>("SNS");
            }
            if (tabela.Table.Columns.Contains("DataAdmissao"))
            {
                this.DataAdmissao = tabela.Field<DateTime>("DataAdmissao");
            }
            if (tabela.Table.Columns.Contains("DataNascimento"))
            {
                this.DataNascimento = tabela.Field<DateTime>("DataNascimento");
            }
            if (tabela.Table.Columns.Contains("Historico"))
            {
                this.Historico = tabela.Field<bool>("Historico");
            }
            if (tabela.Table.Columns.Contains("Tipo"))
            {
                this.Tipo = tabela.Field<bool>("Tipo");
            }
            if (tabela.Table.Columns.Contains("TiposAdmissaoId"))
            {
                this.TiposAdmissaoId = tabela.Field<int>("TiposAdmissaoId");
            }
            if (tabela.Table.Columns.Contains("TipoAdmissao"))
            {
                this.TipoAdmissao = tabela.Field<string>("TipoAdmissao");
            }
            if (tabela.Table.Columns.Contains("MotivoAdmissao"))
            {
                this.MotivoAdmissao = tabela.Field<string>("MotivoAdmissao");
            }
            if (tabela.Table.Columns.Contains("DiagnosticoAdmissao"))
            {
                this.DiagnosticoAdmissao = tabela.Field<string>("DiagnosticoAdmissao");
            }
            if (tabela.Table.Columns.Contains("Observacoes"))
            {
                this.Observacoes = tabela.Field<string>("Observacoes");
            }
            if (tabela.Table.Columns.Contains("NotaAdmissao"))
            {
                this.NotaAdmissao = tabela.Field<string>("NotaAdmissao");
            }
            if (tabela.Table.Columns.Contains("AntecedentesPessoais"))
            {
                this.AntecedentesPessoais = tabela.Field<string>("AntecedentesPessoais");
            }
            if (tabela.Table.Columns.Contains("ExameObjetivo"))
            {
                this.ExameObjetivo = tabela.Field<string>("ExameObjetivo");
            }
            if (tabela.Table.Columns.Contains("Mensalidade"))
            {
                this.Mensalidade = tabela.Field<double>("Mensalidade");
            }
            if (tabela.Table.Columns.Contains("Cofinanciamento"))
            {
                this.Cofinanciamento = tabela.Field<double>("Cofinanciamento");
            }
        }
        #endregion

        #region Outros Métodos

        public static int Inserir(Utentes u)
        {
            string sql;
            sql = "INSERT INTO Utentes (Nome, NIF, SNS, DataAdmissao, DataNascimento, Historico, Tipo, TiposAdmissaoId, MotivoAdmissao, DiagnosticoAdmissao, Observacoes, NotaAdmissao, AntecedentesPessoais, ExameObjetivo, Mensalidade, Cofinanciamento) " +
                  "VALUES ('" + u.Nome + "', " + u.NIF + ", " + u.SNS + ", '" + u.DataAdmissao.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + u.DataNascimento.ToString("yyyy-MM-dd") + "', " + Convert.ToInt32(u.Historico) + ", " + Convert.ToInt32(u.Tipo) + ", " + u.TiposAdmissaoId + ", '" + u.MotivoAdmissao + "', '" + u.DiagnosticoAdmissao + "', '" + u.Observacoes + "', '" + u.NotaAdmissao + "', '" + u.AntecedentesPessoais + "', '" + u.ExameObjetivo + "', " + u.Mensalidade + ", " + u.Cofinanciamento + ")";

            return Geral.Manipular(sql);
        }

        public static int Remover(int id)
        {
            string sql;
            sql = "DELETE FROM Utentes WHERE Id = " + id;

            return Geral.Manipular(sql);
        }

        public static int AlterarDados(Utentes u)
        {
            string sql;
            sql = "UPDATE Utentes SET Nome = '" + u.Nome + "', NIF = " + u.NIF + ", SNS = " + u.SNS + ", DataAdmissao = '" + u.DataAdmissao.ToString("yyyy-MM-dd HH:mm:ss") + "', DataNascimento = '" + u.DataNascimento.ToString("yyyy-MM-dd") + "', " +
                  "Historico = " +u.Historico + ", Tipo = " + u.Tipo + ", TiposAdmissaoId = " + u.TiposAdmissaoId + ", MotivoAdmissao = '" + u.MotivoAdmissao + "', DiagnosticoAdmissao = '" + u.DiagnosticoAdmissao + "', Observacoes = '" + u.Observacoes + "', " +
                  "NotaAdmissao = '" + u.NotaAdmissao + "', AntecedentesPessoais = '" + u.AntecedentesPessoais + "', ExameObjetivo = '" + u.ExameObjetivo + "', Mensalidade = " + u.Mensalidade + ", Cofinanciamento = " + u.Cofinanciamento + " WHERE Id = " + u.Id;

            return Geral.Manipular(sql);
        }

        public static List<Utentes> ObterLista(Dictionary<String, Object> filtros)
        {
            string sql = "SELECT * FROM Utentes where 1=1 ";

            List<Utentes> lstS = Geral<Utentes>.ObterLista(sql);

            return lstS;
        }

        public static Utentes ObterUtente(int id)
        {
            string sql = $"Select u.Id, u.Nome, u.NIF, u.SNS, u.DataAdmissao, u.DataNascimento, u.Historico, u.Tipo, u.TiposAdmissaoId, ta.Descricao TipoAdmissao, u.MotivoAdmissao, u.DiagnosticoAdmissao, u.Observacoes, u.NotaAdmissao, u.AntecedentesPessoais, u.ExameObjetivo, u.Mensalidade, u.Cofinanciamento From Utentes u left join TiposAdmissao ta on ta.Id = u.TiposAdmissaoId where u.Id = {id}";

            Utentes aux = Geral<Utentes>.ObterUnico(sql);

            return aux;
        }


        #endregion

        #endregion
    }
}

