using App_Bets.Application.Dtos;
using App_Bets.Domain.IRepositorio;
using App_Bets.Domain.Modelos;
using App_Bets.Domain.Services;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace App_Bets.Application.Commands.CommandsUsuarios.UpdateUsuario
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, ResultViewModel>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUsuarioContext _usuarioContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UpdateUserHandler(IUnitOfWork unitOfWork, IUsuarioContext usuarioContext, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _usuarioContext = usuarioContext;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<ResultViewModel> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var emailUser = _usuarioContext.Email;

            if (string.IsNullOrWhiteSpace(emailUser))
                return ResultViewModel.Error("Usuário não autenticado.");

            var usuario = await _unitOfWork.UsuarioRepositorio.GetUsuaioEmail(emailUser);
            if (usuario is null)
                return ResultViewModel.Error("Usuário não encontrado.");

            var userIdentity = await _userManager.FindByEmailAsync(emailUser);
            if (userIdentity is null)
                return ResultViewModel.Error("Usuário do Identity não encontrado.");

            usuario.UpdateUsuario(request.DisplayName, request.Email, request.BancaInicial, request.MetaBanca);

            userIdentity.Email = request.Email;
            userIdentity.UserName = request.Email;
            userIdentity.NormalizedEmail = request.Email.ToUpper();
            userIdentity.NormalizedUserName = request.Email.ToUpper();
            userIdentity.DisplayName = request.DisplayName;

            var identityResult = await _userManager.UpdateAsync(userIdentity);
            if (!identityResult.Succeeded)
            {
                var erros = string.Join(", ", identityResult.Errors.Select(e => e.Description));
                return ResultViewModel.Error(erros);
            }

            await _unitOfWork.UsuarioRepositorio.Update(usuario);
            await _unitOfWork.Commit();

            return ResultViewModel.Success();
        }
    }
}
