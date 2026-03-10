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

namespace App_Bets.Application.Queries.Bilhetes.BilhetesPorStatus
{
    public class BilhetePorStatusQuery : ParametrosPaginacao, IRequest<ResultViewModel<List<BilhetesListaPorUsuario>>>
    {
        public StatusEnum StatusEnum { get; set; }

        public BilhetePorStatusQuery(StatusEnum statusEnum, int pageNumber, int pageSize)
        {
            StatusEnum = statusEnum;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
