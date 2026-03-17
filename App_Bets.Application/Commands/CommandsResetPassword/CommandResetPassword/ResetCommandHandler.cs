
using App_Bets.Application.Dtos;
using App_Bets.Domain.IRepositorio;
using App_Bets.Domain.Modelos;
using App_Bets.Domain.Services.EmailServices;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Application.Commands.CommandsResetPassword.CommandResetPassword
{
    public class ResetCommandHandler : IRequestHandler<ResetCommand, ResultViewModel<string>>
    {
        private readonly ISendEmail _sendEmail;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _cache;

        public ResetCommandHandler(ISendEmail sendEmail, IMemoryCache cache, UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {
            _sendEmail = sendEmail;
            _cache = cache;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultViewModel<string>> Handle(ResetCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UsuarioRepositorio.GetUsuaioEmail(request.Email);
            var userIdentity = await _userManager.FindByEmailAsync(request.Email);

            var code = new Random().Next(100000, 999999).ToString();


            if(user is null)
            {
                return ResultViewModel<string>.Error("Email não existe!");
            }
            userIdentity.ResetToken = code;
            userIdentity.ResetTokenExpiration = DateTime.UtcNow.AddMinutes(10);

            await _userManager.UpdateAsync(userIdentity);

            await _sendEmail.ResetPassword(user, code);
            

            return ResultViewModel<string>.Success("Código de recuperação enviado para seu e-mail");

            
        }
    }
}
