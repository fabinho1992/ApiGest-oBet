using App_Bets.Application.Dtos;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Application.FluentValidation
{
    public class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            // 1️⃣ Verifica se existe validator para o Request
            if (_validators.Any())
            {
                // 2️⃣ Cria o contexto de validação
                var context = new ValidationContext<TRequest>(request);

                // 3️⃣ Executa todos os validators
                var failures = _validators
                    .Select(v => v.Validate(context))
                    .SelectMany(r => r.Errors)
                    .Where(e => e != null)
                    .ToList();

                // 4️⃣ Se existir erro, interrompe o fluxo
                if (failures.Any())
                {
                    var mensagem = string.Join(" | ",
                        failures.Select(f => f.ErrorMessage));

                    return (TResponse)(object)
                        ResultViewModel.Error(mensagem);
                }
            }

            // 5️⃣ Se tudo ok, chama o Handler
            return await next();
        }
    }
}
