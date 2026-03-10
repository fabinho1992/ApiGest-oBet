using App_Bets.Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Application.Commands.CommandsBilhetes.DeleteBilhetes
{
    public class DeleteBilheteCommand : IRequest<ResultViewModel<Guid>>
    {
        public Guid Id { get; private set; }

        public DeleteBilheteCommand(Guid id)
        {
            Id = id;
        }
    }
}
