using System;

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
        #endregion

        #endregion
    }
}
