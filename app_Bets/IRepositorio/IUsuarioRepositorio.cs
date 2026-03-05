using App_Bets.Domain.IRepositorio.IGeneric;
using App_Bets.Domain.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Domain.IRepositorio
{
    public interface IUsuarioRepositorio : IGeneric<Usuario>
    {
        Task<Usuario> GetUsuaioEmail(string email);
        Task<Usuario> GetUsuarioCpf(string cpf);    
    }
}
