using App_Bets.Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Application.Commands.CommandsResetPassword.CommandResetPassword
{
    public class ResetCommand : IRequest<ResultViewModel<string>>
    {
        public ResetCommand(string email)
        {
            Email = email;
        }

        public string Email { get; private set; }
    }
}
