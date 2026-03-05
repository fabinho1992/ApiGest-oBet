using App_Bets.Application.Dtos;
using App_Bets.Application.Dtos.Bilhetes;
using App_Bets.Domain.IRepositorio;
using App_Bets.Domain.Modelos;
using App_Bets.Domain.Services;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Application.Queries.Bilhetes.BilhetesPorData
{
    public class BilhetesPorDataHandler : IRequestHandler<BilhetesPorDataQuery, ResultViewModel<List<BilhetesListaPorUsuario>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUsuarioContext _usuarioContext;

        public BilhetesPorDataHandler(IMapper mapper, IUsuarioContext usuarioContext, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _usuarioContext = usuarioContext;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultViewModel<List<BilhetesListaPorUsuario>>> Handle(BilhetesPorDataQuery request, CancellationToken cancellationToken)
        {
            var email = _usuarioContext.Email;

            var (bilhetes, totalCount) =
                await _unitOfWork.BilheteRepositorio
                .GetBilhetesPorData(email, request.Data, request.PageNumber, request.PageSize);

            if (bilhetes == null || !bilhetes.Any())
            {
                return ResultViewModel<List<BilhetesListaPorUsuario>>
                    .Error($"Nenhum bilhete encontrado para está data {request.Data.ToString("d")}.");
            }

            var bilhetesDto =
                _mapper.Map<List<BilhetesListaPorUsuario>>(bilhetes);


            return ResultViewModel<List<BilhetesListaPorUsuario>>
                .Success(bilhetesDto, totalCount);

        }
    }
}
