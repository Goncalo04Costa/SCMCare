using System.Threading.Tasks;

namespace WebApplication1.Servicos
{
    public class NotificacoesServico
    {
        private readonly AppDbContext _context;

        public NotificacoesServico(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExisteNotificacao(int id)
        {
            var notificacao = await _context.Notificacoes.FindAsync(id);
            return notificacao != null;
        }
    }
}
