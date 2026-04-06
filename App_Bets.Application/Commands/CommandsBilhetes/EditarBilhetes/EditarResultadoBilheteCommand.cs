using App_Bets.Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Application.Commands.CommandsBilhetes.EditarBilhetes
{
    public class EditarResultadoBilheteCommand : IRequest<ResultViewModel>
    {
        public Guid BilheteId { get; set; }
        public double NovoValorRetornado { get; set; }
    }
}
