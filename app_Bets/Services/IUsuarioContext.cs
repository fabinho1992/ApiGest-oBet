using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Domain.Services
{
    public interface IUsuarioContext
    {
        string Email { get; }
        string UserId { get; }
    }
}
