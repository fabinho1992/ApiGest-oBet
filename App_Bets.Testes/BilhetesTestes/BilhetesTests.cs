using App_Bets.Application.Commands.CommandsBilhetes.CreateBilhetes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Testes.BilhetesTestes
{
    public class BilhetesTests
    {
        [Fact]
        public async Task CriarBilhete_Valido_DeveCriarComSucesso()
        {
            // Arrange
            //var command = new CreateBilheteCommand
            //{
            //    UsuarioId = Guid.NewGuid(),
            //    ApostaId = Guid.NewGuid(),
            //    ValorAposta = 100,
            //    DataAposta = DateTime.UtcNow
            //};
            //var handler = new CreateBilhetesCommandHandler(/* dependências necessárias */);
            //// Act
            //var result = await handler.Handle(command, CancellationToken.None);
            //// Assert
            //Assert.True(result.IsSuccess);
            //Assert.NotNull(result.Data);
            //Assert.Equal(command.UsuarioId, result.Data.UsuarioId);
            //Assert.Equal(command.ApostaId, result.Data.ApostaId);
            //Assert.Equal(command.ValorAposta, result.Data.ValorAposta);
        }   
    }
}
