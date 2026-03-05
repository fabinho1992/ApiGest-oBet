using App_Bet.Analytics.Interfaces;
using App_Bet.Analytics.Models;
using App_Bets.Application.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Application.Queries.Jogadores
{
    public class JogadoresHandler : IRequestHandler<JogadoresQuery, ResultViewModel<DetalhesJogador>>
    {
        private readonly IBalldontlieClient _balldontlieClient;
        private readonly ILogger<JogadoresHandler> _logger;

        public JogadoresHandler(IBalldontlieClient balldontlieClient, ILogger<JogadoresHandler> logger)
        {
            _balldontlieClient = balldontlieClient;
            _logger = logger;
        }

        public async Task<ResultViewModel<DetalhesJogador>> Handle(JogadoresQuery request, CancellationToken cancellationToken)
        {
            var jogadorDetalhes = await _balldontlieClient.BuscarPlayer(request.Name);
            if (jogadorDetalhes is null)
            {
                return ResultViewModel<DetalhesJogador>.Error("Jogador não encontrado.");
            }

            return ResultViewModel<DetalhesJogador>.Success(jogadorDetalhes);   
        }
    }
}
