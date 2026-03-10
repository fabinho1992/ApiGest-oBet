using App_Bets.Application.Dtos;
using App_Bets.Domain.Enuns;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Application.Commands.CommandsBilhetes.CreateBilhetes
{
    public class CreateBilheteCommand : IRequest<ResultViewModel<Guid>>
    {
        public CreateBilheteCommand(double odd, double valorApostado, TipoBanca tipoBanca,
            StatusEnum statusEnum, CasaAposta casaAposta)
        {
            Odd = odd;
            ValorApostado = valorApostado;
            TipoBanca = tipoBanca;
            StatusEnum = statusEnum;
            CasaAposta = casaAposta;
        }

        public double Odd { get; private set; }
        public double ValorApostado { get; private set; }
        public TipoBanca TipoBanca{ get; private set; }
        public StatusEnum StatusEnum { get; private set; }
        public CasaAposta CasaAposta { get; private set; }
    }
}
