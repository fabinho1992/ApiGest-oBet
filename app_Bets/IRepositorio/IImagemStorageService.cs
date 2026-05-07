using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Domain.IRepositorio
{
    public interface IImagemStorageService
    {
        Task<string> UploadImagemAsync(
            Stream arquivo,
            string nomeArquivo,
            string contentType,
            CancellationToken cancellationToken = default);
    }
}
