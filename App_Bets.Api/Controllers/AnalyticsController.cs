using App_Bet.Analytics.Interfaces;
using App_Bets.Application.Queries.Bilhetes.BilhetesListaConsulta;
using App_Bets.Application.Queries.Jogadores;
using App_Bets.Domain.Modelos;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App_Bets.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AnalyticsController : ControllerBase
    {
        private readonly IBalldontlieClient _client;
        private readonly IMediator _mediator;

        public AnalyticsController(IBalldontlieClient client, IMediator mediator)
        {
            _client = client;
            _mediator = mediator;
        }

        [HttpGet("jogador")]
        public async Task<IActionResult> Analisar([FromQuery] string nome)
        {
            var query = new JogadoresQuery(nome);

            var result = await _mediator.Send(query);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }
    }
}
