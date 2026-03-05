
using App_Bets.Application.Dtos;
using App_Bets.Domain.IServices.Autentication;
using App_Bets.Domain.ModelsAutentication;
using MediatR;

namespace App_Bets.Application.Commands.CommandsAuth.CommandLogin
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, ResultViewModel<ResponseLogin>>
    {
        private readonly ILoginUser _loginUser;

        public LoginCommandHandler(ILoginUser loginUser)
        {
            _loginUser = loginUser;
        }

        public async Task<ResultViewModel<ResponseLogin>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var login = new Login(request.Email, request.Password);
            var result = await _loginUser.LoginAsync(login);

            if (result.Status == "Bad Request 400")
            {
                return ResultViewModel<ResponseLogin>.Error(result.Message);
            }

            return ResultViewModel<ResponseLogin>.Success(result);
        }
    }
}
