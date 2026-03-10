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

namespace App_Bets.Application.Queries.Bilhetes.BilhetesCasaAposta
{
    public class BilhetesCasaApostaHandler : IRequestHandler<BilhetesCasaApostaQuery, ResultViewModel<List<BilhetesListaPorUsuario>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUsuarioContext _usuarioContex;
        private readonly IMapper _mapper;

        public BilhetesCasaApostaHandler(IUnitOfWork unitOfWork, IUsuarioContext usuarioContex, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _usuarioContex = usuarioContex;
            _mapper = mapper;
        }

        public async Task<ResultViewModel<List<BilhetesListaPorUsuario>>> Handle(BilhetesCasaApostaQuery request, CancellationToken cancellationToken)
        {
            var email = _usuarioContex.Email;

            var (bilhetes, totalpaginas, totalCount) =
                await _unitOfWork.BilheteRepositorio
                .GetBilhetesPorCasaAposta(email, request.CasaAposta, request.PageNumber, request.PageSize);

            if (bilhetes == null || !bilhetes.Any())
            {
                return ResultViewModel<List<BilhetesListaPorUsuario>>
                    .Error($"Nenhum bilhete encontrado para está casa de aposta {request.CasaAposta}");
            }

            var bilhetesDto =
                _mapper.Map<List<BilhetesListaPorUsuario>>(bilhetes);


            return ResultViewModel<List<BilhetesListaPorUsuario>>
                .Success(bilhetesDto, totalpaginas);
        }
    }
}
