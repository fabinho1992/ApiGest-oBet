using App_Bets.Application.Dtos;
using App_Bets.Application.Dtos.Bilhetes;
using MediatR;

namespace App_Bets.Application.Queries.Bilhetes.ResumoCasaAposta
{
    public class ResumoCasaApostaQuery : IRequest<ResultViewModel<List<CasaApostaResumoDto>>>
    {

    }
}
