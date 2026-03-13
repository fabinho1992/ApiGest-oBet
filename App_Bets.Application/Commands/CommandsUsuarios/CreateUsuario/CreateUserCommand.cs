using App_Bets.Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Application.Commands.CommandsUser.CreateUsuario
{
    public class CreateUserCommand : IRequest<ResultViewModel>
    {
        public CreateUserCommand(string displayName, string cpf, string email, double bancaInicial, double metaBanca, string password)
        {
            DisplayName = displayName;
            Cpf = cpf;
            Email = email;
            BancaInicial = bancaInicial;
            MetaBanca = metaBanca;
            Password = password;
        }

        public string DisplayName { get; private set; }
        public string Cpf { get; private set; }
        public string Email { get; private set; }
        public double BancaInicial { get; private set; }
        public double MetaBanca { get; private set; }
        public string Password { get; set; }
    }
}
