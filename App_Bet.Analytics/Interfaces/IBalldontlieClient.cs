using App_Bet.Analytics.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bet.Analytics.Interfaces
{
    public interface IBalldontlieClient
    {
        Task<DetalhesJogador?> BuscarPlayer(string nome);
    }
}
