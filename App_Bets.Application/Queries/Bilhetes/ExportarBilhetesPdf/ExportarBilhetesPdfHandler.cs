using App_Bets.Application.Documents;
using App_Bets.Application.Dtos;
using App_Bets.Domain.IRepositorio;
using App_Bets.Domain.Services;
using MediatR;
using QuestPDF.Fluent;

namespace App_Bets.Application.Queries.Bilhetes.ExportarBilhetesPdf
{
    public class ExportarBilhetesPdfHandler
        : IRequestHandler<ExportarBilhetesPdfQuery, ResultViewModel<byte[]>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUsuarioContext _usuarioContext;

        public ExportarBilhetesPdfHandler(
            IUnitOfWork unitOfWork,
            IUsuarioContext usuarioContext)
        {
            _unitOfWork = unitOfWork;
            _usuarioContext = usuarioContext;
        }

        public async Task<ResultViewModel<byte[]>> Handle(
            ExportarBilhetesPdfQuery request,
            CancellationToken cancellationToken)
        {
            var email = _usuarioContext.Email;

            if (string.IsNullOrWhiteSpace(email))
                return ResultViewModel<byte[]>.Error("Usuário não autenticado.");

            var bilhetes = await _unitOfWork.BilheteRepositorio
                .ObterBilhetesFiltradosParaRelatorioAsync(
                    email,
                    request.CasaAposta,
                    request.Mercado,
                    request.Status,
                    request.Data,
                    request.PageNumber,
                    request.PageSize,
                    request.SomentePaginaAtual);

            if (!bilhetes.Any())
                return ResultViewModel<byte[]>.Error("Nenhum bilhete encontrado para gerar o relatório.");

            var filtros = MontarDescricaoFiltros(request);

            var document = new RelatorioBilhetesPdfDocument(
                bilhetes,
                email,
                "Relatório de Bilhetes",
                filtros);

            var pdf = document.GeneratePdf();

            return ResultViewModel<byte[]>.Success(pdf);
        }

        private static string MontarDescricaoFiltros(ExportarBilhetesPdfQuery request)
        {
            var partes = new List<string>();

            if (request.CasaAposta.HasValue)
                partes.Add($"Casa: {request.CasaAposta.Value}");

            if (request.Mercado.HasValue)
                partes.Add($"Mercado: {request.Mercado.Value}");

            if (request.Status.HasValue)
                partes.Add($"Status: {request.Status.Value}");

            if (request.Data.HasValue)
                partes.Add($"Data: {request.Data.Value:dd/MM/yyyy}");

            partes.Add(request.SomentePaginaAtual
                ? $"Página atual: {request.PageNumber}"
                : "Todos os registros filtrados");

            return partes.Any() ? string.Join(" | ", partes) : "Sem filtros";
        }
    }
}