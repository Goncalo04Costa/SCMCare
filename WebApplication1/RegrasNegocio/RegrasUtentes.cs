using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Modelos;
using WebApplication1;

namespace RegrasNegocio
{
    public class RegrasUtentes
    {
        private readonly AppDbContext _context;

        public RegrasUtentes(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> VerificarNIFExistente(int nif)
        {
            // Verifica se existe algum utente com o mesmo NIF no banco de dados
            var utenteExistente = await _context.Utentes.AnyAsync(u => u.NIF == nif);
            return utenteExistente;
        }


    }
}