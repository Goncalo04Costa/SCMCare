using System;
using System.Net.Mail;
using System.Net;

namespace WebApplication1.Servicos
{
    public class CronometroServico: IHostedService, IDisposable
    {
        private Timer _cronometro;
        
        private Timer _cronometroS;


        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cronometro = new Timer(AcaoCronometro, null, TimeSpan.Zero, TimeSpan.FromHours(2));
            _cronometroS = new Timer(AcaoCronometroS, null, TimeSpan.Zero, TimeSpan.FromMinutes(10));
            return Task.CompletedTask;
        }

        public void AcaoCronometro(object state)
        {

        }

        public void AcaoCronometroS(object state)
        {
            //TesteMail();
        }

        private void TesteMail()
        {// Sender's credentials
            string senderEmail = "SCMCareGeral@gmail.com";
            string senderPassword = "cylc quir akgh zqyf";

            // Recipient's email address
            string recipientEmail = "diogoafernandes20@gmail.com";

            // Email subject and body
            string subject = "Teste Email";
            string body = "Email para testar se funciona.\n\n\t" + DateTime.Now;

            // Configure SMTP client
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = true,
            };

            // Create and configure the email message
            MailMessage mailMessage = new MailMessage(senderEmail, recipientEmail, subject, body);

            try
            {
                // Send the email
                smtpClient.Send(mailMessage);
                Console.WriteLine("Email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }
        }



        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cronometro?.Change(Timeout.Infinite, 0);
            _cronometroS?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _cronometro?.Dispose();
            _cronometroS?.Dispose();
        }
    }
}
