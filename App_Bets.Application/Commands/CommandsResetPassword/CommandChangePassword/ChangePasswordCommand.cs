
using App_Bets.Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Application.Commands.CommandsResetPassword.CommandChangePassword
{
    public class ChangePasswordCommand : IRequest<ResultViewModel<string>>
    {
        public ChangePasswordCommand(string email, string code, string password)
        {
            Email = email;
            Code = code;
            Password = password;
        }

        public string Email { get; private set; }
        public string Code { get; private set; }
        public string Password { get; private set; }
    }
}
