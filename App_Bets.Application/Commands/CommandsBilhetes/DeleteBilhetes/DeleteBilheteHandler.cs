using App_Bets.Application.Dtos;
using App_Bets.Domain.IRepositorio;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Application.Commands.CommandsBilhetes.DeleteBilhetes
{
    public class DeleteBilheteHandler : IRequestHandler<DeleteBilheteCommand, ResultViewModel<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteBilheteHandler> _logger;

        public DeleteBilheteHandler(IUnitOfWork unitOfWork, ILogger<DeleteBilheteHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResultViewModel<Guid>> Handle(DeleteBilheteCommand request, CancellationToken cancellationToken)
        {
            var bilhete = await _unitOfWork.BilheteRepositorio.GetById(request.Id);

            if (bilhete is null)
            {
                return ResultViewModel<Guid>.Error("Bilhete não encontrado.");
            }

            await _unitOfWork.BilheteRepositorio.Delete(request.Id);
            await _unitOfWork.Commit();

            return ResultViewModel<Guid>.Success(bilhete.Id);
        }
    }
}
