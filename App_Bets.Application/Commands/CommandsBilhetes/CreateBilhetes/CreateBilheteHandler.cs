using App_Bets.Application.Dtos;
using App_Bets.Domain.IRepositorio;
using App_Bets.Domain.Modelos;
using App_Bets.Domain.Services;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Application.Commands.CommandsBilhetes.CreateBilhetes
{
    public class CreateBilheteHandler : IRequestHandler<CreateBilheteCommand, ResultViewModel<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateBilheteHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IUsuarioContext _usuarioContext;

        public CreateBilheteHandler(IUnitOfWork unitOfWork, ILogger<CreateBilheteHandler> logger, IMapper mapper, IUsuarioContext usuarioContext)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _usuarioContext = usuarioContext;
        }

        public async Task<ResultViewModel<Guid>> Handle(CreateBilheteCommand request, CancellationToken cancellationToken)
        {
            var userId = _usuarioContext.UserId;

            if (string.IsNullOrEmpty(userId))
                return ResultViewModel<Guid>.Error("Usuário não autenticado");

            var bilhete = new Bilhete(request.Odd, request.ValorApostado, request.TipoBanca, request.StatusEnum)
            {
                UsuarioId = Guid.Parse(userId)
            };

            await _unitOfWork.BilheteRepositorio.Add(bilhete);
            await _unitOfWork.Commit();

            return ResultViewModel<Guid>.Success(bilhete.Id);
        }
    }
}
