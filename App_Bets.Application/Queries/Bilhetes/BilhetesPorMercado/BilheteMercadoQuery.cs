using App_Bets.Application.Dtos;
using App_Bets.Application.Dtos.Bilhetes;
using App_Bets.Domain.Enuns;
using App_Bets.Domain.Modelos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Application.Queries.Bilhetes.BilhetesPorMercado
{
    public class BilheteMercadoQuery : ParametrosPaginacao, IRequest<ResultViewModel<List<BilhetesListaPorUsuario>>>
    {
        public BilheteMercadoQuery(int pageNumber, int pageSize, MercadoEnum mercado)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            Mercado = mercado;
        }

        public MercadoEnum Mercado { get; set; }
    }
}
