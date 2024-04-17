using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Modelos;
using WebApplication1;

namespace RegrasNegocio
{
    public class RegrasPratos
    {
        private readonly AppDbContext _context;

        public RegrasPratos(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> VerificarPratoExistente(string nome)
        {

            var PratosExistente = await _context.Pratos.AnyAsync(s => s.Nome == nome);
            return PratosExistente;
        }


    }
}
