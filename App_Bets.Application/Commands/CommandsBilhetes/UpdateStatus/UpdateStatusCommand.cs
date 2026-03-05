using App_Bets.Application.Dtos;
using App_Bets.Domain.Enuns;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Application.Commands.CommandsBilhetes.UpdateStatus
{
    public class UpdateStatusCommand : IRequest<ResultViewModel<Guid>>
    {
        public StatusEnum StatusEnum { get; set; }
        public Guid Id { get; set; }

        public UpdateStatusCommand(StatusEnum statusEnum, Guid id)
        {
            StatusEnum = statusEnum;
            Id = id; 
        }
    }
}
