using App_Bets.Application.Dtos;
using App_Bets.Domain.Enuns;
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
        public CreateUserCommand(string displayName, string cpf, string email, double bancaInicial, double metaBanca, string password, CasaAposta casaPreferida)
        {
            DisplayName = displayName;
            Cpf = cpf;
            Email = email;
            BancaInicial = bancaInicial;
            MetaBanca = metaBanca;
            Password = password;
            CasaPreferida = casaPreferida;
        }

        public string DisplayName { get; private set; }
        public string Cpf { get; private set; }
        public string Email { get; private set; }
        public CasaAposta CasaPreferida { get; set; }
        public double BancaInicial { get; private set; }
        public double MetaBanca { get; private set; }
        public string Password { get; set; }
    }
}
