using App_Bets.Application.Commands.CommandsBilhetes.CreateBilhetes;
using App_Bets.Application.Commands.CommandsBilhetes.DeleteBilhetes;
using App_Bets.Application.Commands.CommandsBilhetes.ResetarBilhetes;
using App_Bets.Application.Commands.CommandsBilhetes.UpdateStatus;
using App_Bets.Application.Queries.Bilhetes.BilhetesCasaAposta;
using App_Bets.Application.Queries.Bilhetes.BilhetesDashboard;
using App_Bets.Application.Queries.Bilhetes.BilhetesListaConsulta;
using App_Bets.Application.Queries.Bilhetes.BilhetesPorData;
using App_Bets.Application.Queries.Bilhetes.BilhetesPorStatus;
using App_Bets.Application.Queries.Bilhetes.BilhetesPorUsuario;
using App_Bets.Application.Queries.Bilhetes.ResumoCasaAposta;
using App_Bets.Application.Queries.Usuario.UsuarioPeloCpf;
using App_Bets.Domain.Enuns;
using App_Bets.Domain.Modelos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace App_Bets.Api.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class BilheteController : ControllerBase
    {
        private readonly ILogger<BilheteController> _logger;
        private readonly IMediator _mediator;
        public BilheteController(ILogger<BilheteController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]  
        public async Task<IActionResult> CreateBilhete([FromBody] CreateBilheteCommand command)
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
            _logger.LogInformation("Bilhete criado com sucesso: {BilheteId}", result.Data);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetByBilhetes([FromQuery] ParametrosPaginacao parametrosPaginacao)
        {
            var query = new BilheteListaQuery(parametrosPaginacao.PageNumber, parametrosPaginacao.PageSize);

            var result = await _mediator.Send(query);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }

        [HttpGet("usuario/bilhetes")]
        public async Task<IActionResult> GetBilhetesUsuario([FromQuery] ParametrosPaginacao parametrosPaginacao)
        {

            var query = new BilhetesPorUsuarioQuery(parametrosPaginacao.PageNumber, parametrosPaginacao.PageSize);

            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }


        [HttpGet("usuario/bilhetesPorData")]
        public async Task<IActionResult> GetBilhetesUsuarioPorData(DateTime data, [FromQuery] ParametrosPaginacao parametrosPaginacao)
        {

            var query = new BilhetesPorDataQuery(data, parametrosPaginacao);

            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpGet("casaAposta")]
        public async Task<IActionResult> GetBilhetesUsuarioPorCasaDeAposta(CasaAposta casaAposta, [FromQuery] ParametrosPaginacao parametrosPaginacao)
        {

            var query = new BilhetesCasaApostaQuery(casaAposta, parametrosPaginacao);

            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetBilhetesDashboard()
        {

            var query = new DashboardQuery();

            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpGet("resumoCasaApostas")]
        public async Task<IActionResult> GetBilhetesResumo()
        {

            var query = new ResumoCasaApostaQuery();

            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpGet("status")]
        public async Task<IActionResult> GetBilhetesStatus(StatusEnum status, [FromQuery] ParametrosPaginacao parametrosPaginacao)
        {

            var query = new BilhetePorStatusQuery(status, parametrosPaginacao.PageNumber, parametrosPaginacao.PageSize);

            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPut("status")]
        public async Task<IActionResult> Update(UpdateStatusCommand updateStatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _mediator.Send(updateStatus);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            return Ok();
        }


        [HttpDelete]
        public async Task<IActionResult> Delete(DeleteBilheteCommand command)
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

            return Ok();
        }


        [HttpDelete("reset")]
        public async Task<IActionResult> DeleteAll()
        {

            var result = await _mediator.Send(new ResetarBilhetesCommand());

            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            return Ok();
        }
    }
}
