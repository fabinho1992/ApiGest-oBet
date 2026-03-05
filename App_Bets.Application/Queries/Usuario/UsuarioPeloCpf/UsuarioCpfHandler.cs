using App_Bets.Application.Dtos;
using App_Bets.Application.Dtos.Usuarios;
using App_Bets.Domain.IRepositorio;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Application.Queries.Usuario.UsuarioPeloCpf
{
    public class UsuarioCpfHandler : IRequestHandler<UsuarioCpfQuery, ResultViewModel<UsuarioDetalhado>>
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IMapper _mapper;

        public UsuarioCpfHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _UnitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResultViewModel<UsuarioDetalhado>> Handle(UsuarioCpfQuery request, CancellationToken cancellationToken)
        {
            var usuario = await _UnitOfWork.UsuarioRepositorio.GetUsuarioCpf(request.Cpf);
            if (usuario == null)
            {
                return ResultViewModel<UsuarioDetalhado>.Error("Usuário não encontrado");

            }

            var usuarioDetalhado = _mapper.Map<UsuarioDetalhado>(usuario);
            return ResultViewModel<UsuarioDetalhado>.Success(usuarioDetalhado);
        }
    }
}
