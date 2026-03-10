using App_Bets.Application.Dtos;
using App_Bets.Application.Dtos.Bilhetes;
using App_Bets.Domain.IRepositorio;
using App_Bets.Domain.Services;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Application.Queries.Bilhetes.BilhetesPorStatus
{
    public class BilhetePorStatusHandler : IRequestHandler<BilhetePorStatusQuery, ResultViewModel<List<BilhetesListaPorUsuario>>>
    {
        private readonly IUsuarioContext _usuarioContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BilhetePorStatusHandler(IUsuarioContext usuarioContext, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _usuarioContext = usuarioContext;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResultViewModel<List<BilhetesListaPorUsuario>>> Handle(BilhetePorStatusQuery request, CancellationToken cancellationToken)
        {
            var email = _usuarioContext.Email;

            var (bilhetes, totalCount) =
                await _unitOfWork.BilheteRepositorio
                    .GetBilhetesPorStatus(email, request.StatusEnum, request.PageNumber, request.PageSize);

            if (bilhetes == null || !bilhetes.Any())
            {
                return ResultViewModel<List<BilhetesListaPorUsuario>>
                    .Error("Nenhum bilhete encontrado com esse Status");
            }

            var bilhetesDto =
                _mapper.Map<List<BilhetesListaPorUsuario>>(bilhetes);


            return ResultViewModel<List<BilhetesListaPorUsuario>>
                .Success(bilhetesDto, totalCount);
        }
    }
}
