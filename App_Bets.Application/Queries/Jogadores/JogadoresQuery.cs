using App_Bet.Analytics.Models;
using App_Bets.Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Application.Queries.Jogadores
{
    public class JogadoresQuery : IRequest<ResultViewModel<DetalhesJogador>>
    {
        public string Name { get; set; }

        public JogadoresQuery(string name)
        {
            Name = name;
        }
    }
}
