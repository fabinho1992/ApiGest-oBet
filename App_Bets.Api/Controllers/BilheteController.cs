using App_Bets.Application.Commands.CommandsBilhetes.CreateBilhetes;
using App_Bets.Application.Commands.CommandsBilhetes.DeleteBilhetes;
using App_Bets.Application.Commands.CommandsBilhetes.ResetarBilhetes;
using App_Bets.Application.Commands.CommandsBilhetes.UpdateStatus;
using App_Bets.Application.Queries.Bilhetes.BilhetesCasaAposta;
using App_Bets.Application.Queries.Bilhetes.BilhetesDashboard;
using App_Bets.Application.Queries.Bilhetes.BilhetesFiltrados;
using App_Bets.Application.Queries.Bilhetes.BilhetesListaConsulta;
using App_Bets.Application.Queries.Bilhetes.BilhetesPorData;
using App_Bets.Application.Queries.Bilhetes.BilhetesPorMercado;
using App_Bets.Application.Queries.Bilhetes.BilhetesPorStatus;
using App_Bets.Application.Queries.Bilhetes.BilhetesPorUsuario;
using App_Bets.Application.Queries.Bilhetes.ExportarBilhetesPdf;
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
        public async Task<IActionResult> GetBilhetesUsuario(MercadoEnum? mercado, [FromQuery] ParametrosPaginacao parametrosPaginacao)
        {

            var query = new BilhetesPorUsuarioQuery(parametrosPaginacao.PageNumber, parametrosPaginacao.PageSize, mercado);

            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }


        [HttpGet("usuario/bilhetesPorData")]
        public async Task<IActionResult> GetBilhetesUsuarioPorData(DateTime data, MercadoEnum? mercado, [FromQuery] ParametrosPaginacao parametrosPaginacao)
        {

            var query = new BilhetesPorDataQuery(data, parametrosPaginacao, mercado);

            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpGet("casaAposta")]
        public async Task<IActionResult> GetBilhetesUsuarioPorCasaDeAposta(CasaAposta casaAposta, MercadoEnum? mercado, [FromQuery] ParametrosPaginacao parametrosPaginacao)
        {

            var query = new BilhetesCasaApostaQuery(casaAposta, parametrosPaginacao, mercado);

            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetBilhetesDashboard(MercadoEnum? mercado)
        {

            var query = new DashboardQuery(mercado);

            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpGet("resumoCasaApostas")]
        public async Task<IActionResult> GetBilhetesResumo(MercadoEnum? mercado)
        {

            var query = new ResumoCasaApostaQuery(mercado);

            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpGet("status")]
        public async Task<IActionResult> GetBilhetesStatus(StatusEnum status, MercadoEnum? mercado, [FromQuery] ParametrosPaginacao parametrosPaginacao)
        {

            var query = new BilhetePorStatusQuery(status, parametrosPaginacao.PageNumber, parametrosPaginacao.PageSize, mercado);

            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPut()]
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


        [HttpGet("mercado")]
        public async Task<IActionResult> GetBilhetesMercados(MercadoEnum mercadoEnum, [FromQuery] ParametrosPaginacao parametrosPaginacao)
        {

            var query = new BilheteMercadoQuery(parametrosPaginacao.PageNumber, parametrosPaginacao.PageSize, mercadoEnum);

            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
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

        [HttpGet("usuario/bilhetes-filtrados")]
        public async Task<IActionResult> GetBilhetesFiltrados(
        [FromQuery] CasaAposta? casaAposta,
        [FromQuery] MercadoEnum? mercado,
        [FromQuery] StatusEnum? status,
        [FromQuery] DateTime? data,
        [FromQuery] ParametrosPaginacao parametrosPaginacao)
        {
            var query = new BilhetesFiltradosQuery(
                parametrosPaginacao.PageNumber,
                parametrosPaginacao.PageSize,
                casaAposta,
                mercado,
                status,
                data);

            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpGet("usuario/bilhetes-relatorio-pdf")]
        public async Task<IActionResult> ExportarBilhetesPdf(
        [FromQuery] CasaAposta? casaAposta,
        [FromQuery] MercadoEnum? mercado,
        [FromQuery] StatusEnum? status,
        [FromQuery] DateTime? data,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] bool somentePaginaAtual = true)
        {
            var query = new ExportarBilhetesPdfQuery(
                casaAposta,
                mercado,
                status,
                data,
                pageNumber,
                pageSize,
                somentePaginaAtual);

            var result = await _mediator.Send(query);

            if (!result.IsSuccess || result.Data is null)
                return BadRequest(result.Message);

            var nomeArquivo = $"relatorio-bilhetes-{DateTime.Now:yyyyMMdd-HHmmss}.pdf";

            return File(result.Data, "application/pdf", nomeArquivo);
        }
    }
}
