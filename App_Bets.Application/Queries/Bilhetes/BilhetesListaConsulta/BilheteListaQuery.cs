using App_Bets.Application.Dtos;
using App_Bets.Application.Dtos.Bilhetes;
using App_Bets.Domain.Modelos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Application.Queries.Bilhetes.BilhetesListaConsulta
{
    public class BilheteListaQuery : ParametrosPaginacao, IRequest<ResultViewModel<List<BilheteLista>>>
    {
        public BilheteListaQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;    
            PageSize = pageSize;
        }
    }
}
