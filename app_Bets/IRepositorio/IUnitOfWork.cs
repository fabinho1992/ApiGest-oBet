using App_Bets.Domain.IServices.Autentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Domain.IRepositorio
{
    public interface IUnitOfWork
    {
        IUsuarioRepositorio UsuarioRepositorio { get; }
        IBilheteRepository BilheteRepositorio { get; }
        Task Commit();
    }
}
