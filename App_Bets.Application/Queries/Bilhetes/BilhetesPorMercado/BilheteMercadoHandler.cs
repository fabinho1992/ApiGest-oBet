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

namespace App_Bets.Application.Queries.Bilhetes.BilhetesPorMercado
{
    public class BilheteMercadoHandler : IRequestHandler<BilheteMercadoQuery, ResultViewModel<List<BilhetesListaPorUsuario>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUsuarioContext _usuarioContex;
        private readonly IMapper _mapper;

        public BilheteMercadoHandler(IUnitOfWork unitOfWork, IUsuarioContext usuarioContex, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _usuarioContex = usuarioContex;
            _mapper = mapper;
        }

        public async Task<ResultViewModel<List<BilhetesListaPorUsuario>>> Handle(BilheteMercadoQuery request, CancellationToken cancellationToken)
        {
            var email = _usuarioContex.Email;

            var (bilhetes, totalPaginas, totalCount) =
                await _unitOfWork.BilheteRepositorio
                    .GetBilhetesPorMercado(email, request.Mercado, request.PageNumber, request.PageSize);

            if (bilhetes == null || !bilhetes.Any())
            {
                return ResultViewModel<List<BilhetesListaPorUsuario>>
                    .Error("Nenhum bilhete encontrado para este mercado!");
            }

            var bilhetesDto =
                _mapper.Map<List<BilhetesListaPorUsuario>>(bilhetes);


            return ResultViewModel<List<BilhetesListaPorUsuario>>
                .Success(bilhetesDto, totalCount);
        }
    }

}

