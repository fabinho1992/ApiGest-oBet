using App_Bets.Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Application.Commands.CommandsUsuarios.UpdateUsuario
{
    public class UpdateUserCommand : IRequest<ResultViewModel>
    {
        public UpdateUserCommand(string email, double bancaInicial, double metaBanca, string displayName)
        {
            Email = email;
            BancaInicial = bancaInicial;
            MetaBanca = metaBanca;
            DisplayName = displayName;
        }

        public string DisplayName { get; private set; }
        public string Email { get; private set; }
        public double BancaInicial { get; private set; }
        public double MetaBanca { get; private set; }
    }
}
