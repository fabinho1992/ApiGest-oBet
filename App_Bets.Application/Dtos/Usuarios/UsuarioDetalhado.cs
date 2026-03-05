using App_Bets.Application.Dtos.bilhetes;
using App_Bets.Domain.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Application.Dtos.Usuarios
{
    public class UsuarioDetalhado
    {
        public Guid Id { get;  set; }
        public string DisplayName { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public double BancaAtual { get; set; }
        public double MetaBanca { get; set; }
        public string DataCriacao { get; set; }

        public ICollection<BilheteConsultaCpf> Bilhetes { get; set; }
    }
}
