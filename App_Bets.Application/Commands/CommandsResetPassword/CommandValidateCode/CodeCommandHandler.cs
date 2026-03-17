
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

namespace App_Bets.Application.Commands.CommandsResetPassword.CommandValidateCode
{
    public class CodeCommandHandler : IRequestHandler<ValidateCodeCommand, ResultViewModel<string>>
    {
        private readonly IMemoryCache _cache;
        private readonly UserManager<ApplicationUser> _userManager;

        public CodeCommandHandler(IMemoryCache cache, UserManager<ApplicationUser> userManager)
        {
            _cache = cache;
            _userManager = userManager;
        }

        public async Task<ResultViewModel<string>> Handle(ValidateCodeCommand request, CancellationToken cancellationToken)
        {
            var keyCache = $"RecoveryCode:{request.Email}";

            if (!_cache.TryGetValue(keyCache, out string? code) || code != request.Code)
            {
                Console.WriteLine($"Esse é o código :{code}");
                return ResultViewModel<string>.Error("Código não está válido!");
            }

            return ResultViewModel<string>.Success($"{request.Code}, {request.Email}");
        }
    }
}
