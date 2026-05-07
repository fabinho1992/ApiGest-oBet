using App_Bets.Application.Dtos;
using App_Bets.Domain.Enuns;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Application.Commands.CommandsBilhetes.CreateBilhetes
{
    public class CreateBilheteCommand : IRequest<ResultViewModel<Guid>>
    {

        public double Odd { get; set; }
        public double ValorApostado { get; set; }
        public TipoBanca TipoBanca { get; set; }
        public StatusEnum StatusEnum { get; set; }
        public CasaAposta CasaAposta { get; set; }
        public MercadoEnum Mercado { get; set; }
        public DateTime? DataAposta { get; set; }
        public IFormFile? Imagem { get; set; }
    }
}
