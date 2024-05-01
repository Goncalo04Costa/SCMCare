using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Modelos;
using WebApplication1;

namespace RegrasNegocio
{
    public class RegrasSenhas
    {
        private readonly AppDbContext _context;

        public RegrasSenhas(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> VerificarReservaExistente(int funcionarioId, int menuId)
        {
            var senhaReservada = await _context.Senhas.FirstOrDefaultAsync(s => s.FuncionariosId == funcionarioId && s.MenuId == menuId);
            return senhaReservada != null;
        }
    }
}