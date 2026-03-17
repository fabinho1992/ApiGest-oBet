
using App_Bets.Domain.Services.EmailServices;
using Microsoft.Extensions.Configuration;
using MimeKit;
using SendGrid;
using SendGrid.Helpers.Mail;
#region 
//namespace App_Bets.Application.Services.EmailServices
//{
//    public class EmailService : IEmailService
//    {
//        private readonly IConfiguration _config;

//        public EmailService(IConfiguration config)
//        {
//            _config = config;
//        }

//        public Task SendEmailService(string subject, string message, string userEmail, string userName)
//        {
//            // Configure as credenciais do servidor SMTP
//            //var smtpServer = "smtp.gmail.com"; //"smtp.gmail.com"; // Substitua pelo seu servidor SMTP
//            //var smtpPort = 587; // Substitua pela porta do seu servidor SMTP
//            //var smtpUsername = "f.santosdev1992@gmail.com";//"f.santosdev1992@gmail.com"; // Substitua pelo seu email
//            //var smtpPassword = "ruzm otfz iwde ddej";//"ruzm otfz iwde ddej"; // Substitua pela sua senha
//            var smtpServer = _config["SmtpSettings:Server"];
//            var smtpPort = int.Parse(_config["SmtpSettings:Port"]);
//            var smtpUsername = _config["SmtpSettings:User"];
//            var smtpPassword = _config["SmtpSettings:Pass"];


//            // Crie um novo email
//            var mimeMessage = new MimeMessage();
//            mimeMessage.From.Add(MailboxAddress.Parse(smtpUsername));
//            mimeMessage.To.Add(MailboxAddress.Parse(userEmail));
//            mimeMessage.Subject = subject;

//            // Crie o corpo do email
//            var bodyBuilder = new BodyBuilder();
//            bodyBuilder.HtmlBody = message; // Use HtmlBody para obter o corpo em HTML
//            mimeMessage.Body = bodyBuilder.ToMessageBody();

//            // Converta o MimeMessage para MailMessage
//            var mailMessage = new MailMessage();
//            mailMessage.From = new MailAddress(mimeMessage.From.ToString());
//            mailMessage.To.Add(new MailAddress(mimeMessage.To.ToString()));
//            mailMessage.Subject = mimeMessage.Subject;
//            mailMessage.Body = mimeMessage.HtmlBody; // Use HtmlBody para definir o corpo do email

//            // Envie o email
//            var client = new SmtpClient(smtpServer, smtpPort);
//            client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
//            client.EnableSsl = true; // Ative o SSL para o Gmail
//            client.Send(mailMessage);

//            return Task.CompletedTask;
//        }
//    }
//}

//using System.Net.Http.Headers;
//using System.Text;
//using System.Text.Json;
//using Microsoft.Extensions.Configuration;
//using App_Bets.Domain.Services.EmailServices;

//namespace App_Bets.Application.Services.EmailServices
//{
//    public class EmailService : IEmailService
//    {
//        private readonly IConfiguration _config;
//        private readonly HttpClient _httpClient;

//        public EmailService(IConfiguration config, HttpClient httpClient)
//        {
//            _config = config;
//            _httpClient = httpClient;
//        }

//        public async Task SendEmailService(string subject, string message, string userEmail, string userName)
//        {
//            var apiKey = _config["MailerSend:ApiKey"];
//            var fromEmail = _config["MailerSend:FromEmail"];
//            var fromName = _config["MailerSend:FromName"];

//            var request = new
//            {
//                from = new { email = fromEmail, name = fromName },
//                to = new[]
//                {
//                    new { email = userEmail, name = userName }
//                },
//                subject = subject,
//                html = message
//            };

//            var json = JsonSerializer.Serialize(request);

//            var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://api.mailersend.com/v1/email");
//            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
//            httpRequest.Content = new StringContent(json, Encoding.UTF8, "application/json");

//            var response = await _httpClient.SendAsync(httpRequest);

//            if (!response.IsSuccessStatusCode)
//            {
//                var error = await response.Content.ReadAsStringAsync();
//                throw new Exception($"Erro MailerSend: {error}");
//            }
//        }
//    }
//}
#endregion

namespace App_Bets.Application.ServiceEmail.EmailServices       
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailService(string subject, string toEmail, string userName, string message, bool isHtml = false)
        {
            var apiKey = _config["SendGrid:ApiKey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(_config["SendGrid:FromEmail"], "Gerenciamento de bancas");
            var to = new EmailAddress(toEmail, userName);

            // Se isHtml for verdadeiro, use a mensagem como HTML
            var plainTextContent = isHtml ? null : message; // Se não for HTML, use a mensagem
            var htmlContent = isHtml ? message : null; // Se for HTML, use a mensagem

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
            Console.WriteLine(response.StatusCode);
        }
    }
}