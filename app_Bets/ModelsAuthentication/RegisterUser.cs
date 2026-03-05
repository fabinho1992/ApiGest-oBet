using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Domain.ModelsAutentication
{
    public class RegisterUser
    {
        public RegisterUser(string userName, string email, string password, Guid usuarioId)
        {
            UserName = userName;
            Email = email;
            Password = password;
            UsuarioId = usuarioId;
        }

        public string UserName { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public Guid UsuarioId { get; private set; }
    }
}
