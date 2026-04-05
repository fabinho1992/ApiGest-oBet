using App_Bets.Domain.IRepositorio;
using App_Bets.Domain.Modelos;
using App_Bets.Domain.Services.EmailServices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace App_Bets.Application.Services.EmailServices
{
    public class SendEmail : ISendEmail
    {
        IEmailService _emailService;
        IUnitOfWork _unitOfWork;
        ILogger<Bilhete> _logger;

        public SendEmail(IEmailService emailService, IUnitOfWork unitOfWork, ILogger<Bilhete> logger)
        {
            _emailService = emailService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task ResetPassword(Usuario usuario, string code)
        {
            var message = $@"
                        Olá {usuario.DisplayName},

            Recebemos uma solicitação para redefinir sua senha no <strong>App Bets</strong>.

            Seu código de recuperação é:

            <h2 style=""letter-spacing:4px"">{code}</h2>

            Este código expira em 10 minutos.

            Se você não solicitou a redefinição de senha, ignore este email.

            Atenciosamente,  
            Equipe App Bets
            ";
            await _emailService.SendEmailService(
                "Redefinição de Senha",
                usuario.Email,
                usuario.DisplayName,
                message,
                true
            );

            Console.WriteLine("Método de envio executado");
        }

        public async Task SendEmailConfirmation(Usuario user)
        {
            var message = $@"
                <h2>Bem-vindo ao BetVisions 🎉</h2>

                <p>Olá <strong>{user.DisplayName}</strong>,</p>

                <p>
                Seu cadastro foi realizado com sucesso!
                </p>

                <p>
                Agora você já pode acessar a plataforma e começar a acompanhar
                suas apostas, estatísticas e evolução da sua banca.
                </p>

                <p>
                Se você não realizou este cadastro, apenas ignore este e-mail.
                </p>

                <br/>

                <p>
                Bons jogos! ⚽
                </p>

                <p>
                <strong>Equipe App Bets</strong>
                </p>
                ";

            await _emailService.SendEmailService(
                "Confirmação de cadastro",
                user.Email,
                user.DisplayName,
                message,
                isHtml: true
            );

            _logger.LogInformation(message);
        }
    }
}
