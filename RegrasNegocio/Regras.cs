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

            Sobremesas.ObterLista(dic);

            return 0;
        }
    }
}
