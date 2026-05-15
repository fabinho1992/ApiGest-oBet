using App_Bets.Domain.Enuns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Application.Dtos.Bilhetes
{
    public class BilheteExtraidoDto
    {
        public List<double>? OddsIndividuais { get; set; }
        public double? Odd { get; set; }
        public double? ValorApostado { get; set; }
        public string Mercado { get; set; }
        public StatusEnum Status { get; set; }
        public string? CasaAposta { get; set; }
        public DateTime? DataAposta { get; set; }
    }
}
