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

namespace App_Bets.Application.Queries.Bilhetes.BilhetesFiltrados
{
    public class BilhetesFiltradosHandler
       : IRequestHandler<BilhetesFiltradosQuery, ResultViewModel<List<BilhetesListaPorUsuario>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUsuarioContext _usuarioContext;

        public BilhetesFiltradosHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IUsuarioContext usuarioContext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _usuarioContext = usuarioContext;
        }

        public async Task<ResultViewModel<List<BilhetesListaPorUsuario>>> Handle(
            BilhetesFiltradosQuery request,
            CancellationToken cancellationToken)
        {
            var email = _usuarioContext.Email;

            if (string.IsNullOrWhiteSpace(email))
            {
                return ResultViewModel<List<BilhetesListaPorUsuario>>.Error("Usuário não autenticado.");
            }

            var (bilhetes, totalPaginas) =
                await _unitOfWork.BilheteRepositorio.ObterBilhetesFiltradosPorUsuarioAsync(
                    email,
                    request.PageNumber,
                    request.PageSize,
                    request.CasaAposta,
                    request.Mercado,
                    request.Status,
                    request.Data);

            if (!bilhetes.Any())
            {
                return ResultViewModel<List<BilhetesListaPorUsuario>>.Error("Nenhum bilhete encontrado para os filtros informados.");
            }

            var bilhetesDto = _mapper.Map<List<BilhetesListaPorUsuario>>(bilhetes);

            return ResultViewModel<List<BilhetesListaPorUsuario>>.Success(bilhetesDto, totalPaginas);
        }
    }
}
