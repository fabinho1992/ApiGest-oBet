using App_Bets.Application.Dtos;
using App_Bets.Application.Dtos.Bilhetes;
using App_Bets.Domain.Modelos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Application.Queries.Bilhetes.BilhetesPorData
{
    public class BilhetesPorDataQuery : ParametrosPaginacao, IRequest<ResultViewModel<List<BilhetesListaPorUsuario>>>
    {
        public DateTime Data { get; set; }

        public BilhetesPorDataQuery(DateTime data, ParametrosPaginacao parametros)
        {
            Data = data;
            PageNumber = parametros.PageNumber;
            PageSize = parametros.PageSize; 
        }
    }
}
