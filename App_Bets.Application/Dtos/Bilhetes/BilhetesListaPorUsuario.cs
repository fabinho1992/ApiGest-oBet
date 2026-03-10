using App_Bets.Domain.Enuns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Application.Dtos.Bilhetes
{
    public class BilhetesListaPorUsuario
    {
        public Guid Id { get; private set; }
        public string UsuarioNome { get; set; }
        public double Odd { get; private set; }
        public StatusEnum Status { get; set; }
        public TipoBanca TipoBanca { get; set; }
        public CasaAposta CasaAposta { get; set; }
        public double ValorApostado { get; private set; } 
        public double ValorRetornado { get; private set; }
        public string DataAposta { get; private set; }
    }
}
