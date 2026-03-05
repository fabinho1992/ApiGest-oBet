using App_Bets.Application.Dtos;
using App_Bets.Domain.IRepositorio;
using App_Bets.Domain.IServices.Autentication;
using App_Bets.Domain.Modelos;
using App_Bets.Domain.ModelsAutentication;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Application.Commands.CommandsUser.CreateUsuario
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, ResultViewModel<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICreateUser _createUser;

        public CreateUserHandler(IUnitOfWork unitOfWork, IMapper mapper, ICreateUser createUser)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createUser = createUser;
        }

        public async Task<ResultViewModel<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var usuario = _mapper.Map<Usuario>(request);
            await _unitOfWork.UsuarioRepositorio.Add(usuario);
            await _unitOfWork.Commit();

            var identityUser = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                UsuarioId = usuario.Id
            };

            var registerUser = new RegisterUser(request.DisplayName, request.Email, request.Password, usuario.Id);

            var result = await _createUser.CreateUserAsync(registerUser);

            if (result.Status == "Erro" )
            {
                return ResultViewModel<Guid>.Error("Falha ao criar usuário identity");
            }

            return new ResultViewModel<Guid>(usuario.Id);
        }
    }
}
