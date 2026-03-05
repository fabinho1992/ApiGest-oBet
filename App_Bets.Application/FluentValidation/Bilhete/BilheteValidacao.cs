using App_Bets.Application.Commands.CommandsBilhetes.CreateBilhetes;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Application.FluentValidation.Bilhete
{
    public class BilheteValidacao : AbstractValidator<CreateBilheteCommand>  
    {
        public BilheteValidacao()
        {
            RuleFor(x => x.Odd)
                .GreaterThan(1.0).WithMessage("A odd deve ser maior que 1.0.");
            RuleFor(x => x.ValorApostado)
                .GreaterThan(0).WithMessage("O valor apostado deve ser maior que zero.");
        }   
    }
}
