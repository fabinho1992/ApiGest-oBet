using App_Bets.Application.Commands.CommandsUser.CreateUsuario;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Application.FluentValidation.Usuario
{
    public class CriacaoValidacao : AbstractValidator<CreateUserCommand>
    {
        public CriacaoValidacao()
        {
            RuleFor(x => x.DisplayName)
           .NotEmpty().WithMessage("O nome do usuário é obrigatório.")
           .MinimumLength(3).WithMessage("O nome deve ter no mínimo 3 caracteres.")
           .MaximumLength(100).WithMessage("O nome deve ter no máximo 100 caracteres.");

            RuleFor(x => x.Cpf)
                .NotEmpty().WithMessage("O CPF é obrigatório.")
                .Length(11).WithMessage("O CPF deve conter 11 dígitos.")
                .Matches("^[0-9]*$").WithMessage("O CPF deve conter apenas números.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O email é obrigatório.")
                .EmailAddress().WithMessage("O email informado é inválido.")
                .MaximumLength(120).WithMessage("O email deve ter no máximo 120 caracteres.");

            RuleFor(x => x.BancaInicial)
                .GreaterThanOrEqualTo(0).WithMessage("A banca inicial não pode ser negativa.");

            RuleFor(x => x.MetaBanca)
                .GreaterThan(0).WithMessage("A meta da banca deve ser maior que zero.")
                .GreaterThan(x => x.BancaInicial)
                .WithMessage("A meta deve ser maior que a banca inicial.");

            RuleFor(x => x.Password)
    .           NotEmpty().WithMessage("A senha é obrigatória");
        }
    }
}
