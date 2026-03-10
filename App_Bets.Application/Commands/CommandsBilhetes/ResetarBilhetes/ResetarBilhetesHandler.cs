using App_Bets.Application.Dtos;
using App_Bets.Domain.IRepositorio;
using App_Bets.Domain.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Application.Commands.CommandsBilhetes.ResetarBilhetes
{
    public class ResetarBilhetesHandler : IRequestHandler<ResetarBilhetesCommand, ResultViewModel<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUsuarioContext _usuarioContext;

        public ResetarBilhetesHandler(IUnitOfWork unitOfWork, IUsuarioContext usuarioContext)
        {
            _unitOfWork = unitOfWork;
            _usuarioContext = usuarioContext;
        }

        public async Task<ResultViewModel<bool>> Handle(ResetarBilhetesCommand request, CancellationToken cancellationToken)
        {
            var email = _usuarioContext.Email;

            if (string.IsNullOrEmpty(email))
                return ResultViewModel<bool>.Error("Usuário não autenticado");

            await _unitOfWork.BilheteRepositorio.DeleteAll(email);

            await _unitOfWork.Commit();

            return ResultViewModel<bool>.Success(true);


        }
    }
}
