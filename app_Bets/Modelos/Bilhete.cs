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
        public Bilhete(double odd, double valorApostado, TipoBanca tipoBanca, StatusEnum status, MercadoEnum mercado, DateTime? dataAposta = null, string? imagemUrl = null)
        {
            Odd = odd;
            ValorApostado = valorApostado;
            TipoBanca = odd <= 2 ? TipoBanca.Segura : tipoBanca;
            Status = status;
            CasaAposta = CasaAposta.Betano;
            ValorRetornado = CalcularValorRetorno();
            Mercado = mercado;
            DataAposta = dataAposta ?? DateTime.UtcNow;
            ImagemUrl = imagemUrl;
        }

        public Guid Id { get; private set; }
        public Guid UsuarioId { get; set; }
        public double Odd { get; private set; }
        public StatusEnum Status { get; set; }
        public TipoBanca TipoBanca { get; set; }
        public CasaAposta CasaAposta { get; private set; }
        public MercadoEnum Mercado { get; private set; }
        public double ValorApostado { get; private set; }
        public double ValorRetornado { get; private set; }
        public DateTime? DataAposta { get; private set; }
        public string? ImagemUrl { get; private set; }
        public Usuario? Usuario { get; set; }


        public double CalcularValorRetorno()
        {
            return Odd * ValorApostado;
        }

        public void AtualizarStatus(StatusEnum novoStatus)
        {
            Status = novoStatus;
        }

        public void AtualizarCasaAposta(CasaAposta novaCasaAposta)
        {
            CasaAposta = novaCasaAposta;
        }

        public void AtualizarValorRetornado(double novoValorRetornado)
        {
            if (novoValorRetornado < 0)
                throw new ArgumentException("O valor retornado não pode ser negativo.");

            ValorRetornado = novoValorRetornado;
        }

        public double ObterImpactoNaBanca()
        {
            return Status switch
            {
                StatusEnum.Ganha => ValorRetornado - ValorApostado,
                StatusEnum.Perdida => -ValorApostado,
                _ => 0
            };
        }

        public void AtualizarImagem(string? imagemUrl)
        {
            ImagemUrl = imagemUrl;
        }

    }
}
