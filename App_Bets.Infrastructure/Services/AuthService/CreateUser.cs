using App_Bets.Domain.IServices.Autentication;
using App_Bets.Domain.Modelos;
using App_Bets.Domain.ModelsAutentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReviewManager.Infrastructure.Service.Identity
{
    public class CreateUser : ICreateUser
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateUser(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ResponseIdentityCreate> CreateUserAsync(RegisterUser registerUser)
        {
            var usuarioExiste = await _userManager.FindByEmailAsync(registerUser.Email!);//consulto se o nome passado exite no banco

            if (usuarioExiste != null)
            {
                return new ResponseIdentityCreate { Status = "Erro", Message = "User already exists!" };// se existir passo esse erro
            }
            ApplicationUser user = new()
            {
                UserName = registerUser.Email,
                Email = registerUser.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UsuarioId = registerUser.UsuarioId  
            };

            var resultado = await _userManager.CreateAsync(user, registerUser.Password!);//crio o usuario

            if (!resultado.Succeeded)
            {
                return new ResponseIdentityCreate { Message = "Erro", Status = "Error creating user" };
            }

            return new ResponseIdentityCreate { Status = "Ok", Message = "User created successfully" };
        }
    }
}
