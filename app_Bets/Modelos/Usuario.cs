
using App_Bets.Domain.Enuns;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Domain.Modelos
{
    public class Usuario
    {
        public Usuario(string nome, string cpf, string email, CasaAposta casaPreferida , double bancaInicial, double metaBanca)
        {
            DisplayName = nome;
            Cpf = cpf;
            Email = email;
            BancaPreferida = casaPreferida;
            BancaInicial = bancaInicial;
            MetaBanca = metaBanca;
            DataCriacao = DateTime.UtcNow;
            BancaAtual = bancaInicial;
        }

        public Guid Id { get; private set; }
        public string DisplayName { get; private set; }
        public string Cpf { get; private set; }
        public string Email { get; private set; }
        public CasaAposta BancaPreferida { get; set; }
        public double BancaInicial { get; private set; }
        public double BancaAtual { get; private set; }
        public double MetaBanca { get; private set; }
        public DateTime DataCriacao { get; private set; }

        public ICollection<Bilhete> Bilhetes { get; private set; } = new List<Bilhete>();

        protected Usuario() { }


        public void DebitarPerda(double valor)
        {
            BancaAtual -= valor;
        }

        public void CreditarGanho(double valor)
        {
            BancaAtual += valor;
        }

        public void UpdateUsuario(string? displayName, string? email, double bancaInicial, double metaBanca)
        {
            DisplayName = displayName;
            Email = email;
            BancaInicial = bancaInicial;
            BancaAtual = bancaInicial; 
            MetaBanca = metaBanca;
        }

        public void ZerarBanca() 
        {
            BancaAtual = BancaInicial;
        }

    }
}
