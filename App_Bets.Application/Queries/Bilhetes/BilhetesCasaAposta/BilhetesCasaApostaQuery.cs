using App_Bets.Application.Dtos;
using App_Bets.Application.Dtos.Bilhetes;
using App_Bets.Domain.Enuns;
using App_Bets.Domain.Modelos;
using MediatR;

namespace App_Bets.Application.Queries.Bilhetes.BilhetesCasaAposta
{
    public class BilhetesCasaApostaQuery : ParametrosPaginacao, IRequest<ResultViewModel<List<BilhetesListaPorUsuario>>>
    {
        public BilhetesCasaApostaQuery(CasaAposta casaAposta, ParametrosPaginacao parametrosPaginacao, MercadoEnum? mercado)
        {
            PageNumber = parametrosPaginacao.PageNumber;
            PageSize = parametrosPaginacao.PageSize;
            CasaAposta = casaAposta;
            Mercado = mercado;
        }

        public CasaAposta CasaAposta { get; private set; }
        public MercadoEnum? Mercado { get; set; }
    }
}
