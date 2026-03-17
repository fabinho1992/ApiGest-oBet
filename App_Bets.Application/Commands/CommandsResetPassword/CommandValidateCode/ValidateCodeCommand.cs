
using App_Bets.Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Application.Commands.CommandsResetPassword.CommandValidateCode
{
    public class ValidateCodeCommand : IRequest<ResultViewModel<string>>
    {
        public ValidateCodeCommand(string email, string code)
        {
            Email = email;
            Code = code;
        }

        public string Email { get; private set; }
        public string Code { get; private set; }
    }
}
