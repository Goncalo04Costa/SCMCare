using ObjetosNegocio;
using System;
using System.Collections.Generic;

namespace RegrasNegocio
{
    public class Regras
    {
        public static int Teste()
        {
            Dictionary<String, Object> dic = new Dictionary<String, Object>();
            dic.Add("Nome", "Ban");

            Utentes.ObterLista(dic);

            //Sobremesas a = new Sobremesas();
            //a.Nome = "Teste2";
            //a.Descricao = "Teste inserir sobremesa 2";
            //a.Tipo = false;

            //return Sobremesas.Inserir(a);

            return 0;
        }

        public static object ObterFichaUtente(int id)
        {
            Utentes u = new Utentes();
            u = Utentes.ObterUtente(id);

            return u;
        }

        public static int RegistaAvaliacaoUtente(Avaliacoes a)
        {
            return Avaliacoes.Inserir(a);
        }
    }
}
