

using App_Bets.Domain.Modelos;

namespace App_Bets.Domain.Services.EmailServices
{
    public interface ISendEmail
    {
        Task SendEmailConfirmation(Usuario usuario);
        Task ResetPassword(Usuario usuario, string code);
    }
}
