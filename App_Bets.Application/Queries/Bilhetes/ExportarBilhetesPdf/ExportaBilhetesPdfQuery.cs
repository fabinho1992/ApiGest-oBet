using App_Bets.Application.Dtos;
using App_Bets.Domain.Enuns;
using MediatR;

namespace App_Bets.Application.Queries.Bilhetes.ExportarBilhetesPdf
{
    public class ExportarBilhetesPdfQuery : IRequest<ResultViewModel<byte[]>>
    {
        public ExportarBilhetesPdfQuery(
            CasaAposta? casaAposta,
            MercadoEnum? mercado,
            StatusEnum? status,
            DateTime? data,
            int pageNumber,
            int pageSize,
            bool somentePaginaAtual)
        {
            CasaAposta = casaAposta;
            Mercado = mercado;
            Status = status;
            Data = data;
            PageNumber = pageNumber;
            PageSize = pageSize;
            SomentePaginaAtual = somentePaginaAtual;
        }

        public CasaAposta? CasaAposta { get; set; }
        public MercadoEnum? Mercado { get; set; }
        public StatusEnum? Status { get; set; }
        public DateTime? Data { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public bool SomentePaginaAtual { get; set; } = true;
    }
}