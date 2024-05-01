using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Modelos;
using WebApplication1;

namespace RegrasNegocio
{
    public class RegrasSobremesas
    {
        private readonly AppDbContext _context;

        public RegrasSobremesas(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> VerificarSobremesaExistente(string nome)
        {

            var SobremesaExistente = await _context.Sobremesas.AnyAsync(s => s.Nome == nome);
            return SobremesaExistente;
        }


    }
}