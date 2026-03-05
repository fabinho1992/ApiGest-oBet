using App_Bets.Domain.Enuns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Application.Dtos.bilhetes
{
    public class BilheteConsultaCpf
    {
        public Guid Id { get; private set; }
        public string Usuarioname { get; set; }
        public double Odd { get; private set; }
        public StatusEnum Status { get; set; }
        public double ValorApostado { get; private set; }
        public double ValorRetornado { get; private set; }
        public string DataAposta { get; private set; }
    }
}
