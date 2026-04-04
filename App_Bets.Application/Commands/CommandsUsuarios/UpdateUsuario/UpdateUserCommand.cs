using App_Bets.Application.Dtos;
using App_Bets.Domain.Enuns;
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
        public UpdateUserCommand(string email, double bancaInicial, double metaBanca, string displayName, CasaAposta casaPreferida)
        {
            Email = email;
            BancaInicial = bancaInicial;
            MetaBanca = metaBanca;
            DisplayName = displayName;
            CasaPreferida = casaPreferida;
        }

        public string DisplayName { get; private set; }
        public string Email { get; private set; }
        public CasaAposta CasaPreferida { get; set; }
        public double BancaInicial { get; private set; }
        public double MetaBanca { get; private set; }
    }
}
