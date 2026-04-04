using App_Bets.Application.Dtos;
using App_Bets.Domain.IRepositorio;
using App_Bets.Domain.Services;
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
        private readonly IUsuarioContext _usuarioContext;

        public DeleteBilheteHandler(IUnitOfWork unitOfWork, ILogger<DeleteBilheteHandler> logger, IUsuarioContext usuarioContext)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _usuarioContext = usuarioContext;
        }

        public async Task<ResultViewModel<Guid>> Handle(DeleteBilheteCommand request, CancellationToken cancellationToken)
        {
            var email = _usuarioContext.Email;

            if (string.IsNullOrEmpty(email))
                return ResultViewModel<Guid>.Error("Usuário não autenticado");

            var usuario = await _unitOfWork.UsuarioRepositorio.GetUsuaioEmail(email);
            if (usuario is null)
                return ResultViewModel<Guid>.Error("Usuário não encontrado");

            var bilhete = await _unitOfWork.BilheteRepositorio.GetById(request.Id);
            if (bilhete is null)
                return ResultViewModel<Guid>.Error("Bilhete não encontrado");

            if (bilhete.UsuarioId != usuario.Id)
                return ResultViewModel<Guid>.Error("Bilhete não pertence ao usuário");

            var impacto = bilhete.ObterImpactoNaBanca();

            // desfaz o efeito do bilhete na banca
            usuario.AjustarBanca(-impacto);

            await _unitOfWork.BilheteRepositorio.Delete(bilhete.Id);
            _unitOfWork.UsuarioRepositorio.Update(usuario);

            await _unitOfWork.Commit();

            return ResultViewModel<Guid>.Success(bilhete.Id);
        }
    }
}
