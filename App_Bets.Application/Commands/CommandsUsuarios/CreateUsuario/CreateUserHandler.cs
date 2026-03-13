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
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, ResultViewModel>
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

        public async Task<ResultViewModel> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var usuario = _mapper.Map<Usuario>(request);
            await _unitOfWork.UsuarioRepositorio.Add(usuario);
            await _unitOfWork.Commit();

            var registerUser = new RegisterUser(request.DisplayName, request.Email, request.Password, usuario.Id);
            var result = await _createUser.CreateUserAsync(registerUser);

            // Se falhou na criação do usuário identity
            if (result.Status == "Erro")
            {
                return ResultViewModel.Error("Falha ao criar usuário identity");
            }

            // Sucesso → retorna Id do usuário
            return ResultViewModel.Success();
        }
    }
    
}
