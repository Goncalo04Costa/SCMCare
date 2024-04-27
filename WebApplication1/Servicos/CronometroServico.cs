using System;
using System.Net.Mail;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace WebApplication1.Servicos
{
    public class CronometroServico : IHostedService, IDisposable
    {
        private Timer _cronometro;
        private const int InatividadeTimeoutMinutos = 1;
        private DateTime _ultimaAtividade;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cronometro = new Timer(CheckInatividade, null, TimeSpan.Zero, TimeSpan.FromMinutes(1)); 
            _ultimaAtividade = DateTime.Now; 
            return Task.CompletedTask;
        }

        private void CheckInatividade(object state)
        {
            // Verifique se o tempo desde a última atividade excede o tempo limite de inatividade
            if (!VerificarAtividadeRecente())
            {
                // Se exceder, pare a aplicação
                FecharAPI();
            }
        }

        private void FecharAPI()
        {
            // Coloque aqui o código para fechar a API
            // Por exemplo, você pode usar Environment.Exit(0) para encerrar o processo da aplicação
            Environment.Exit(0);
        }

        // Método chamado para registrar a atividade da aplicação
        private void RegistrarAtividade()
        {
            _ultimaAtividade = DateTime.Now; // Atualize o tempo da última atividade para o momento atual
        }

        // Implemente a lógica para verificar a atividade recente da aplicação
        // Este método deve ser chamado sempre que houver uma interação ou atividade na aplicação
        private bool VerificarAtividadeRecente()
        {
            // Verifique se houve alguma atividade nos últimos minutos definidos em InatividadeTimeoutMinutos
            return DateTime.Now.Subtract(_ultimaAtividade).TotalMinutes <= InatividadeTimeoutMinutos;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cronometro?.Change(Timeout.Infinite, 0);
            // Aqui você pode adicionar código adicional de desligamento, se necessário
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _cronometro?.Dispose();
        }
    }
}
