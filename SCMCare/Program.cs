using RegrasNegocio;
using System;

namespace SCMCare
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Regras.Teste());
            Regras.ObterFichaUtente(2);
        }
    }
}
