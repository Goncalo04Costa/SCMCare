using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Servicos
{
    public class TiposFuncionarioServico
    {
        private readonly AppDbContext _context;

        public TiposFuncionarioServico(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> ObterTipoPorNome(string tipo)
        {
            var tipoFuncionario = await _context.TiposFuncionario.FirstOrDefaultAsync(f => f.Descricao == tipo);
            if (tipoFuncionario == null)
                return -1;
            return tipoFuncionario.Id;
        }
    }
}
