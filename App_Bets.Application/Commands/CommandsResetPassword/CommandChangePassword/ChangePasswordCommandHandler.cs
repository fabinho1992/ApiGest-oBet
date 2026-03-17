
using App_Bets.Application.Commands.CommandsResetPassword.CommandChangePassword;
using App_Bets.Application.Dtos;
using App_Bets.Domain.Modelos;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Commands.CommandsResetPassword.CommandChangePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, ResultViewModel<string>>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ChangePasswordCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ResultViewModel<string>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            // 1. Validar o código de recuperação
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user.ResetToken != request.Code)
            {
                return ResultViewModel<string>.Error("Código inválido.");
            }

            if (user.ResetTokenExpiration < DateTime.UtcNow)
            {
                return ResultViewModel<string>.Error("Código expirado.");
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            
            var result = await _userManager.ResetPasswordAsync(user, resetToken, request.Password);

            if (!result.Succeeded)
            {
                return ResultViewModel<string>.Error("Falha ao alterar senha");
            }

            user.ResetToken = "";

            await _userManager.UpdateAsync(user);

            // 5. Retornar sucesso
            return ResultViewModel<string>.Success("Senha alterada com sucesso!");
        }
    }
}
