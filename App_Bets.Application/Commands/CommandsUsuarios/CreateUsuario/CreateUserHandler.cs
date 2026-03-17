using App_Bets.Application.Dtos;
using App_Bets.Domain.IRepositorio;
using App_Bets.Domain.IServices.Autentication;
using App_Bets.Domain.Modelos;
using App_Bets.Domain.ModelsAutentication;
using App_Bets.Domain.Services.EmailServices;
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
        private readonly ISendEmail _sendEmail;

        public CreateUserHandler(IUnitOfWork unitOfWork, IMapper mapper, ICreateUser createUser, ISendEmail sendEmail)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createUser = createUser;
            _sendEmail = sendEmail;
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

            try
            {
                await _sendEmail.SendEmailConfirmation(usuario);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao enviar email: {ex.Message}");
            }

            // Sucesso → retorna Id do usuário
            return ResultViewModel.Success();
        }
    }
    
}
