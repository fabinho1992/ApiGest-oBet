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

namespace App_Bets.Application.Queries.Bilhetes.ResumoCasaAposta
{
    public class ResumoCasaApostaHandler : IRequestHandler<ResumoCasaApostaQuery, ResultViewModel<List<CasaApostaResumoDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUsuarioContext _usuarioContex;
        private readonly IMapper _mapper;

        public ResumoCasaApostaHandler(IUnitOfWork unitOfWork, IUsuarioContext usuarioContex, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _usuarioContex = usuarioContex;
            _mapper = mapper;
        }

        public async Task<ResultViewModel<List<CasaApostaResumoDto>>> Handle(ResumoCasaApostaQuery request, CancellationToken cancellationToken)
        {
            var email = _usuarioContex.Email;
            var resumo = await _unitOfWork.BilheteRepositorio.GetResumoCasas(email);

            var resumoDto = _mapper.Map<List<CasaApostaResumoDto>>(resumo);
            Console.WriteLine(resumoDto);

            if (!resumo.Any())
            {
                return ResultViewModel<List<CasaApostaResumoDto>>
                    .Success(resumoDto);
            }


            return ResultViewModel<List<CasaApostaResumoDto>>
                .Success(resumoDto);

        }
    }
}
