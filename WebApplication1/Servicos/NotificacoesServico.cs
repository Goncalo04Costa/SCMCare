using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;
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

        public async Task<int> InserirNotificacao(Notificacao notificacao)
        {
            if (notificacao == null)
            {
                return 0;
            }

            _context.Notificacoes.Add(notificacao);
            await _context.SaveChangesAsync();

            return notificacao.Id;
        }

        public async Task<int> InserirNotificacaoTipoFuncionario(int NotificacaoId, int TipoFuncionarioId)
        {
            bool existe = await ExisteNotificacao(NotificacaoId);
            if (!existe)
            {
                return 0;
            }

            var funcionarios = await _context.Funcionarios.Where(f => f.TiposFuncionarioId == TipoFuncionarioId).ToListAsync();

            foreach (var funcionario in funcionarios)
            {
                var notificacaoFuncionario = new NotificacaoFuncionario
                {
                    NotificacaoId = NotificacaoId,
                    FuncionarioId = funcionario.FuncionarioID,
                    Estado = 0
                };

                _context.NotificacoesFuncionario.Add(notificacaoFuncionario);
            }
            await _context.SaveChangesAsync();

            return 1;
        }
    }
}
