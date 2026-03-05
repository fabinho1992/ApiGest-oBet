using App_Bets.Application.Commands.CommandsUser.CreateUsuario;
using App_Bets.Application.Queries.Usuario.UsuarioPeloCpf;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App_Bets.Api.Controllers
{
    
    [Route("[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly ILogger<UsuarioController> _logger;
        private readonly IMediator _mediator;

        public UsuarioController(ILogger<UsuarioController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUsuario([FromBody] CreateUserCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            _logger.LogInformation("Usuário criado com sucesso: {Email}", command.Email);
            return Ok(result);
        }

        [HttpGet("cpf")]
        [Authorize]
        public async Task<IActionResult> GetByCpf([FromQuery] string cpf)
        {
            var query = new UsuarioCpfQuery(cpf);

            var result = await _mediator.Send(query);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }
        
        [HttpGet("teste")]
        [Authorize]
        public IActionResult Teste()
        {
            return Ok("OK");
        }
    }
}
