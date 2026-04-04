using App_Bets.Application.Dtos;
using App_Bets.Application.Dtos.Bilhetes;
using App_Bets.Domain.Enuns;
using MediatR;

namespace App_Bets.Application.Queries.Bilhetes.ResumoCasaAposta
{
    public class ResumoCasaApostaQuery : IRequest<ResultViewModel<List<CasaApostaResumoDto>>>
    {
        public MercadoEnum? Mercado { get; set; }

        public ResumoCasaApostaQuery(MercadoEnum? mercado)
        {
            Mercado = mercado;
        }
    }
}
