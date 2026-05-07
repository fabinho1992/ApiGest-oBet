using App_Bets.Application.Commands.CommandsBilhetes.CreateBilhetes;
using App_Bets.Domain.Enuns;
using App_Bets.Domain.IRepositorio;
using App_Bets.Domain.Modelos;
using App_Bets.Domain.Services;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace App_Bets.Tests.Application.Commands.CommandsBilhetes
{
    public class CreateBilheteHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<CreateBilheteHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUsuarioContext> _usuarioContextMock;
        private readonly Mock<IImagemStorageService> _IImagemStorageService;    
        private readonly Mock<IBilheteRepository> _bilheteRepositorioMock;
        private readonly Mock<IUsuarioRepositorio> _usuarioRepositorioMock;

        private readonly CreateBilheteHandler _handler;

        public CreateBilheteHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<CreateBilheteHandler>>();
            _mapperMock = new Mock<IMapper>();
            _usuarioContextMock = new Mock<IUsuarioContext>();
            _IImagemStorageService = new Mock<IImagemStorageService>(); 
            _bilheteRepositorioMock = new Mock<IBilheteRepository>();
            _usuarioRepositorioMock = new Mock<IUsuarioRepositorio>();

            _unitOfWorkMock.Setup(x => x.BilheteRepositorio).Returns(_bilheteRepositorioMock.Object);
            _unitOfWorkMock.Setup(x => x.UsuarioRepositorio).Returns(_usuarioRepositorioMock.Object);

            _handler = new CreateBilheteHandler(
                _unitOfWorkMock.Object,
                _loggerMock.Object,
                _mapperMock.Object,
                _usuarioContextMock.Object,
                _IImagemStorageService.Object
            );
        }

        [Fact]
        public async Task Handle_DeveRetornarErro_QuandoUsuarioNaoForEncontrado()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var command = CriarCommand(StatusEnum.Pendente);

            _usuarioContextMock.Setup(x => x.UserId).Returns(userId);
            _usuarioRepositorioMock
                .Setup(x => x.GetById(It.IsAny<Guid>()))
                .ReturnsAsync((Usuario?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Usuário não encontrado", result.Message);

            _bilheteRepositorioMock.Verify(x => x.Add(It.IsAny<Bilhete>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.Commit(), Times.Never);
        }

        [Fact]
        public async Task Handle_DeveCriarBilheteEFazerCommit_QuandoDadosForemValidos()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = CriarCommand(StatusEnum.Pendente);
            var usuario = CriarUsuarioFake();

            _usuarioContextMock.Setup(x => x.UserId).Returns(userId.ToString());
            _usuarioRepositorioMock.Setup(x => x.GetById(userId)).ReturnsAsync(usuario);

            Bilhete? bilheteCapturado = null;

            _bilheteRepositorioMock
                .Setup(x => x.Add(It.IsAny<Bilhete>()))
                .Callback<Bilhete>(b =>
                {
                    bilheteCapturado = b;
                    SetPrivateGuidProperty(b, "Id", Guid.NewGuid());
                })
                .Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(x => x.Commit()).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotEqual(Guid.Empty, result.Data);

            Assert.NotNull(bilheteCapturado);
            Assert.Equal(userId, bilheteCapturado!.UsuarioId);
            Assert.Equal(command.Odd, bilheteCapturado.Odd);
            Assert.Equal(command.ValorApostado, bilheteCapturado.ValorApostado);
            Assert.Equal(command.StatusEnum, bilheteCapturado.Status);
            Assert.Equal(command.CasaAposta, bilheteCapturado.CasaAposta);

            _bilheteRepositorioMock.Verify(x => x.Add(It.IsAny<Bilhete>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.Commit(), Times.Once);
        }

        [Fact]
        public async Task Handle_DeveCreditarGanho_QuandoStatusForGanha()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = CriarCommand(StatusEnum.Ganha, odd: 2.0, valorApostado: 100);
            var usuario = CriarUsuarioFake();

            var bancaAntes = usuario.BancaAtual;
            var lucroEsperado = (command.Odd * command.ValorApostado) - command.ValorApostado;

            _usuarioContextMock.Setup(x => x.UserId).Returns(userId.ToString());
            _usuarioRepositorioMock.Setup(x => x.GetById(userId)).ReturnsAsync(usuario);

            _bilheteRepositorioMock
                .Setup(x => x.Add(It.IsAny<Bilhete>()))
                .Callback<Bilhete>(b => SetPrivateGuidProperty(b, "Id", Guid.NewGuid()))
                .Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(x => x.Commit()).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(bancaAntes + lucroEsperado, usuario.BancaAtual);

            _bilheteRepositorioMock.Verify(x => x.Add(It.IsAny<Bilhete>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.Commit(), Times.Once);
        }

        [Fact]
        public async Task Handle_DeveDebitarPerda_QuandoStatusForPerdida()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = CriarCommand(StatusEnum.Perdida, odd: 2.0, valorApostado: 100);
            var usuario = CriarUsuarioFake();

            var bancaAntes = usuario.BancaAtual;

            _usuarioContextMock.Setup(x => x.UserId).Returns(userId.ToString());
            _usuarioRepositorioMock.Setup(x => x.GetById(userId)).ReturnsAsync(usuario);

            _bilheteRepositorioMock
                .Setup(x => x.Add(It.IsAny<Bilhete>()))
                .Callback<Bilhete>(b => SetPrivateGuidProperty(b, "Id", Guid.NewGuid()))
                .Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(x => x.Commit()).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(bancaAntes - command.ValorApostado, usuario.BancaAtual);

            _bilheteRepositorioMock.Verify(x => x.Add(It.IsAny<Bilhete>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.Commit(), Times.Once);
        }

        [Fact]
        public async Task Handle_NaoDeveAlterarBanca_QuandoStatusForPendente()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = CriarCommand(StatusEnum.Pendente, odd: 2.0, valorApostado: 100);
            var usuario = CriarUsuarioFake();

            var bancaAntes = usuario.BancaAtual;

            _usuarioContextMock.Setup(x => x.UserId).Returns(userId.ToString());
            _usuarioRepositorioMock.Setup(x => x.GetById(userId)).ReturnsAsync(usuario);

            _bilheteRepositorioMock
                .Setup(x => x.Add(It.IsAny<Bilhete>()))
                .Callback<Bilhete>(b => SetPrivateGuidProperty(b, "Id", Guid.NewGuid()))
                .Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(x => x.Commit()).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(bancaAntes, usuario.BancaAtual);

            _bilheteRepositorioMock.Verify(x => x.Add(It.IsAny<Bilhete>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.Commit(), Times.Once);
        }

        private static CreateBilheteCommand CriarCommand(
        StatusEnum status,
        double odd = 2.0,
        double valorApostado = 100)
        {
            return new CreateBilheteCommand()
            {
                Odd = odd,
                ValorApostado = valorApostado,
                TipoBanca = TipoBanca.Bingo,
                StatusEnum = status,
                CasaAposta = CasaAposta.Betano,
                Mercado = MercadoEnum.Gols
            };
        }

        private static Usuario CriarUsuarioFake()
        {
            return new Usuario(
                nome: "Fabio",
                cpf: "12345678900",
                email: "fabio@email.com",
                casaPreferida: CasaAposta.Betano,
                bancaInicial: 1000,
                metaBanca: 2000
            );
        }

        private static void SetPrivateGuidProperty(object obj, string propertyName, Guid value)
        {
            var property = obj.GetType().GetProperty(propertyName);
            property?.SetValue(obj, value);
        }
    }
}