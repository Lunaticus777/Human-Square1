using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Human_Evolution.Services
{
    /// <summary>
    /// Envoi des e-mails du site.
    /// Railway bloque le SMTP sur l'offre Hobby : on passe donc par l'API HTTPS de Brevo
    /// quand la variable d'environnement BREVO_API_KEY est définie (Railway > Variables).
    /// Sans clé, on retombe sur le SMTP (utile en local).
    /// </summary>
    public class MailService
    {
        // Destinataires de tous les formulaires du site
        public static readonly string[] Recipients = { "geral@human-square.com" };
        public static readonly string[] CcRecipients = { "geralvelho@gmail.com" };

        // Expéditeur (doit être validé comme « sender » dans Brevo)
        private const string SenderEmail = "geral@human-square.com";
        private const string SenderName = "Human Square – Site web";

        private static readonly HttpClient Http = new HttpClient { Timeout = TimeSpan.FromSeconds(20) };

        private readonly SmtpSettings _settings;

        public MailService(IOptions<SmtpSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendEmailAsync(string subject, string body, string replyTo = null)
        {
            var apiKey = Environment.GetEnvironmentVariable("BREVO_API_KEY");
            if (!string.IsNullOrWhiteSpace(apiKey))
            {
                await SendWithBrevoAsync(apiKey, subject, body, replyTo);
                return;
            }
            await SendWithSmtpAsync(subject, body, replyTo);
        }

        private static async Task SendWithBrevoAsync(string apiKey, string subject, string htmlBody, string replyTo)
        {
            var payload = new Dictionary<string, object>
            {
                ["sender"] = new { name = SenderName, email = SenderEmail },
                ["to"] = Recipients.Select(e => new { email = e }).ToArray(),
                ["subject"] = subject,
                ["htmlContent"] = htmlBody
            };
            if (CcRecipients.Length > 0)
                payload["cc"] = CcRecipients.Select(e => new { email = e }).ToArray();
            if (!string.IsNullOrWhiteSpace(replyTo))
                payload["replyTo"] = new { email = replyTo };

            using var request = new HttpRequestMessage(HttpMethod.Post, "https://api.brevo.com/v3/smtp/email")
            {
                Content = JsonContent.Create(payload)
            };
            request.Headers.Add("api-key", apiKey);
            request.Headers.Add("accept", "application/json");

            HttpResponseMessage response;
            try
            {
                response = await Http.SendAsync(request);
            }
            catch (TaskCanceledException)
            {
                throw new Exception("le service d'e-mail ne répond pas, réessayez dans un instant.");
            }

            if (!response.IsSuccessStatusCode)
            {
                var detail = await response.Content.ReadAsStringAsync();
                throw new Exception($"Brevo a refusé l'envoi ({(int)response.StatusCode}) : {detail}");
            }
        }

        private async Task SendWithSmtpAsync(string subject, string body, string replyTo)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Human Square", _settings.From));
            foreach (var r in Recipients) message.To.Add(MailboxAddress.Parse(r));
            foreach (var r in CcRecipients) message.Cc.Add(MailboxAddress.Parse(r));
            message.Subject = subject;
            message.Body = new BodyBuilder { HtmlBody = body }.ToMessageBody();
            if (!string.IsNullOrWhiteSpace(replyTo))
                message.ReplyTo.Add(new MailboxAddress(replyTo, replyTo));

            using var smtp = new SmtpClient { Timeout = 15000 };
            try
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
                await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls, cts.Token);
                await smtp.AuthenticateAsync(_settings.User, _settings.Password, cts.Token);
                await smtp.SendAsync(message, cts.Token);
                await smtp.DisconnectAsync(true, cts.Token);
            }
            catch (MailKit.Security.AuthenticationException ex)
            {
                throw new Exception("Échec de l'authentification SMTP : vérifiez le login et le mot de passe.", ex);
            }
            catch (OperationCanceledException ex)
            {
                throw new Exception("le serveur d'e-mail ne répond pas.", ex);
            }
            catch (System.Exception ex)
            {
                throw new Exception("Erreur lors de l'envoi du message : " + ex.Message, ex);
            }
        }
    }
}
