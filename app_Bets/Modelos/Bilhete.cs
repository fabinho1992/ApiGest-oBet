using App_Bets.Domain.Enuns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Domain.Modelos
{
    public class Bilhete
    {
        public Bilhete(double odd, double valorApostado, TipoBanca tipoBanca, StatusEnum status)
        {
            Odd = odd;
            ValorApostado = valorApostado;
            TipoBanca = tipoBanca;
            Status = status;
            ValorRetornado = CalcularValorRetorno();
            DataAposta = DateTime.Now;
        }

        public Guid Id { get; private set; }
        public Guid UsuarioId { get; set; }
        public double Odd { get; private set; }
        public StatusEnum Status { get; set; }
        public TipoBanca TipoBanca { get; set; }

        public double ValorApostado { get; private set; }
        public double ValorRetornado { get; private set; }
        public DateTime DataAposta { get; private set; }
        public Usuario? Usuario { get; set; }


        public double CalcularValorRetorno()
        {
            return Odd * ValorApostado;
        }

        public void AtualizarStatus(StatusEnum novoStatus)
        {
            Status = novoStatus;
        }

    }
}
