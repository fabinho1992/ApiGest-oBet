using App_Bets.Application.Dtos;
using App_Bets.Application.Dtos.Bilhetes;
using App_Bets.Domain.IRepositorio;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Application.Queries.Bilhetes.BilhetesListaConsulta
{
    public class BilheteListaHandler : IRequestHandler<BilheteListaQuery, ResultViewModel<List<BilheteLista>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public BilheteListaHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResultViewModel<List<BilheteLista>>> Handle(BilheteListaQuery request, CancellationToken cancellationToken)
        {
            var (bilhetes, totalCount) =
            await _unitOfWork.BilheteRepositorio.GetAll(request);

            if (bilhetes == null || !bilhetes.Any())
            {
                return ResultViewModel<List<BilheteLista>>
                    .Error("Bilhetes não encontrados.");
            }

            
            var bilhetesDto = _mapper.Map<List<BilheteLista>>(bilhetes);

            return ResultViewModel<List<BilheteLista>>
                .Success(bilhetesDto, totalCount);
        }
    }
}
