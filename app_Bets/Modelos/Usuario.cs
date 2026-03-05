
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
        public Usuario(string nome, string cpf, string email, double bancaInicial, double metaBanca)
        {
            DisplayName = nome;
            Cpf = cpf;
            Email = email;
            BancaInicial = bancaInicial;
            MetaBanca = metaBanca;
            DataCriacao = DateTime.Now;
            BancaAtual = bancaInicial;
        }

        public Guid Id { get; private set; }
        public string DisplayName { get; private set; }
        public string Cpf { get; private set; }
        public string Email { get; private set; }

        public double BancaInicial { get; private set; }
        public double BancaAtual { get; private set; }
        public double MetaBanca { get; private set; }
        public DateTime DataCriacao { get; private set; }

        public ICollection<Bilhete> Bilhetes { get; private set; } = new List<Bilhete>();

        protected Usuario() { }

        
    }
}
