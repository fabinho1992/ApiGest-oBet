using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Application.Dtos.Bilhetes
{
    public class BilheteAnaliseImagemResponse
    {
        public Guid BilheteId { get; set; }
        public BilheteExtraidoDto DadosExtraidos { get; set; } = new();
    }
}
