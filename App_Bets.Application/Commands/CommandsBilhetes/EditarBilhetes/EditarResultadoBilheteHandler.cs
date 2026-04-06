using App_Bets.Application.Commands.CommandsBilhetes.EditarBilhetes;
using App_Bets.Application.Dtos;
using App_Bets.Domain.Enuns;
using App_Bets.Domain.IRepositorio;
using App_Bets.Domain.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace App_Bets.Application.Commands.CommandsBilhetes.EditarResultadoBilhete
{
    public class EditarResultadoBilheteHandler : IRequestHandler<EditarResultadoBilheteCommand, ResultViewModel>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<EditarResultadoBilheteHandler> _logger;
        private readonly IUsuarioContext _usuarioContext;

        public EditarResultadoBilheteHandler(
            IUnitOfWork unitOfWork,
            ILogger<EditarResultadoBilheteHandler> logger,
            IUsuarioContext usuarioContext)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _usuarioContext = usuarioContext;
        }

        public async Task<ResultViewModel> Handle(EditarResultadoBilheteCommand request, CancellationToken cancellationToken)
        {
            var userId = _usuarioContext.UserId;

            if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(userId, out var usuarioId))
                return ResultViewModel.Error("Usuário não autenticado.");

            var usuario = await _unitOfWork.UsuarioRepositorio.GetById(usuarioId);
            if (usuario is null)
                return ResultViewModel.Error("Usuário não encontrado.");

            var bilhete = await _unitOfWork.BilheteRepositorio.GetById(request.BilheteId);
            if (bilhete is null)
                return ResultViewModel.Error("Bilhete não encontrado.");

            if (bilhete.UsuarioId != usuarioId)
                return ResultViewModel.Error("Você não tem permissão para editar este bilhete.");

            if (bilhete.Status != StatusEnum.Ganha)
                return ResultViewModel.Error("Só é permitido editar o valor retornado de bilhetes com status Ganha.");

            if (request.NovoValorRetornado < 0)
                return ResultViewModel.Error("O valor retornado não pode ser negativo.");

            var impactoAntigo = bilhete.ObterImpactoNaBanca();

            bilhete.AtualizarValorRetornado(request.NovoValorRetornado);

            var impactoNovo = bilhete.ObterImpactoNaBanca();
            var diferenca = impactoNovo - impactoAntigo;

            usuario.AjustarBanca(diferenca);

            await _unitOfWork.UsuarioRepositorio.Update(usuario);
            await _unitOfWork.BilheteRepositorio.Update(bilhete);
            await _unitOfWork.Commit();

            _logger.LogInformation(
                "Bilhete {BilheteId} editado com sucesso. ValorRetornado antigo impactava {ImpactoAntigo}, novo impacto {ImpactoNovo}, diferença aplicada {Diferenca}.",
                bilhete.Id, impactoAntigo, impactoNovo, diferenca);

            return ResultViewModel.Success();
        }
    }
}