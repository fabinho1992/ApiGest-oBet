using App_Bets.Domain.Enuns;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Application.Dtos.Bilhetes
{
    public class ConfirmarBilheteImagemRequest
    {
        public double Odd { get; set; }
        public double ValorApostado { get; set; }
        public CasaAposta CasaAposta { get; set; }
        public MercadoEnum Mercado { get; set; }
        public DateTime DataAposta { get; set; }
        public IFormFile? Imagem { get; set; }
    }
}
