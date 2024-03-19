using Objetos;
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

            //Sobremesas.ObterLista(dic);
            Sobremesas a = new Sobremesas();
            a.Nome = "Teste2";
            a.Descricao = "Teste inserir sobremesa 2";
            a.Tipo = false;

            return Sobremesas.Inserir(a);

            //return 0;
        }
    }
}
