using App_Bets.Application.Dtos;
using App_Bets.Domain.Enuns;
using App_Bets.Domain.IRepositorio;
using App_Bets.Domain.Services;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Application.Commands.CommandsBilhetes.UpdateStatus
{
    public class UpdateStatusHandler : IRequestHandler<UpdateStatusCommand, ResultViewModel<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateStatusHandler> _logger;
        private readonly IUsuarioContext _usuarioContext;

        public UpdateStatusHandler(IUnitOfWork unitOfWork, ILogger<UpdateStatusHandler> logger, IUsuarioContext usuarioContext)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _usuarioContext = usuarioContext;
        }

        public async Task<ResultViewModel<Guid>> Handle(UpdateStatusCommand request, CancellationToken cancellationToken)
        {
            var email = _usuarioContext.Email;
            var user = await _unitOfWork.UsuarioRepositorio.GetUsuaioEmail(email);
            var bilhete = await _unitOfWork.BilheteRepositorio.GetById(request.Id);
            if(bilhete is null)
            {
                return ResultViewModel<Guid>.Error("Bilhete não encontrado.");
            }
            _logger.LogInformation($"Bilhete encontrado: {bilhete.Id}, Status atual: {bilhete.Status}");

            double lucro = bilhete.ValorRetornado - bilhete.ValorApostado;
            if(request.StatusEnum == StatusEnum.Ganha)
            {
                user.CreditarGanho(lucro);
            }else if(request.StatusEnum == StatusEnum.Perdida)
                user.DebitarPerda(bilhete.ValorApostado);

            bilhete.AtualizarStatus(request.StatusEnum);

            await _unitOfWork.UsuarioRepositorio.Update(user);
            await _unitOfWork.BilheteRepositorio.Update(bilhete);
            await _unitOfWork.Commit();

            return ResultViewModel<Guid>.Success(bilhete.Id);
        }
    }
}
