using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bet.Analytics.Models
{
    public class DetalhesJogador
    {
        public int Id { get; set; }
        public string NomeCompleto { get; set; } = string.Empty;
        public string Posicao { get; set; } = string.Empty;
        public string Altura { get; set; } = string.Empty;
        public string Peso { get; set; } = string.Empty;
        public string Camisa { get; set; } = string.Empty;
        public string Faculdade { get; set; } = string.Empty;
        public string Pais { get; set; } = string.Empty;
        public string DraftInfo { get; set; } = string.Empty;
        public string TimeAtual { get; set; } = string.Empty;
    }
}
