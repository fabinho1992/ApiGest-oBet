using App_Bets.Application.Dtos.Bilhetes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App_Bets.Application.Services.IAClaude
{
    public interface IClaudeAnaliseBilheteService
    {
        Task<BilheteExtraidoDto> AnalisarImagemAsync(
            IFormFile imagem,
            CancellationToken cancellationToken = default);
    }
}
