using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Domain.Modelos
{
    public class Dashboard
    {
        public int TotalGanhas { get; set; }
        public int TotalPerdidas { get; set; }
        public double Lucro { get; set; }
        public double Prejuizo { get; set; }
        public double TotalInvestido { get; set; }
        public double ResultadoFinal => Lucro - Prejuizo;

    }
}
