using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Application.Dtos.Bilhetes
{
    public class DashboardDto
    {
        public int TotalGanhas { get; set; }
        public int TotalPerdidas { get; set; }
        public double Lucro { get; set; }
        public double Prejuizo { get; set; }
        public double TotalInvestido { get; set; }
        public double ResultadoFinal { get; set; }
        public string ROI { get; set; }
    }
}
