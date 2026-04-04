using App_Bets.Application.Dtos;
using App_Bets.Application.Dtos.Bilhetes;
using App_Bets.Domain.Enuns;
using App_Bets.Domain.Modelos;
using MediatR;

namespace App_Bets.Application.Queries.Bilhetes.BilhetesFiltrados
{
    public class BilhetesFiltradosQuery : ParametrosPaginacao, IRequest<ResultViewModel<List<BilhetesListaPorUsuario>>>
    {
        public BilhetesFiltradosQuery(
            int pageNumber,
            int pageSize,
            CasaAposta? casaAposta,
            MercadoEnum? mercado,
            StatusEnum? status,
            DateTime? data)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            CasaAposta = casaAposta;
            Mercado = mercado;
            Status = status;
            Data = data;
        }

        public CasaAposta? CasaAposta { get; set; }
        public MercadoEnum? Mercado { get; set; }
        public StatusEnum? Status { get; set; }
        public DateTime? Data { get; set; }
    }
}