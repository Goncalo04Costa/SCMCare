using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Modelos;
using WebApplication1;

namespace RegrasNegocio
{
    public class RegrasSopas
    {
        private readonly AppDbContext _context;

        public RegrasSopas(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> VerificarSopaExistente(string nome)
        {

            var SopaExistente = await _context.Sopas.AnyAsync(s => s.Nome == nome);
            return SopaExistente;
        }


    }
}