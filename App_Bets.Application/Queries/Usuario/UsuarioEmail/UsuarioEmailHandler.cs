using App_Bets.Application.Dtos;
using App_Bets.Application.Dtos.Usuarios;
using App_Bets.Application.Queries.Usuario.UsuarioPeloCpf;
using App_Bets.Domain.IRepositorio;
using App_Bets.Domain.Services;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Application.Queries.Usuario.UsuarioEmail
{
    public class UsuarioEmailHandler : IRequestHandler<UsuarioEmailQuery, ResultViewModel<UsuarioDetalhado>>
    {
        private readonly IUsuarioContext _usuarioContext;
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IMapper _mapper;

        public UsuarioEmailHandler(IUnitOfWork unitOfWork, IMapper mapper, IUsuarioContext usuarioContext)
        {
            _UnitOfWork = unitOfWork;
            _mapper = mapper;
            _usuarioContext = usuarioContext;
        }

        public async Task<ResultViewModel<UsuarioDetalhado>> Handle(UsuarioEmailQuery request, CancellationToken cancellationToken)
        {
            var email = _usuarioContext.Email;

            var usuario = await _UnitOfWork.UsuarioRepositorio.GetUsuaioEmail(email);
            if (usuario == null)
            {
                return ResultViewModel<UsuarioDetalhado>.Error("Usuário não encontrado");

            }

            var usuarioDetalhado = _mapper.Map<UsuarioDetalhado>(usuario);
            return ResultViewModel<UsuarioDetalhado>.Success(usuarioDetalhado);
        }
    }
}
