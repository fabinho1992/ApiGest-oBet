using App_Bets.Application.Dtos;
using App_Bets.Application.Dtos.Bilhetes;
using App_Bets.Application.Queries.Bilhetes.BilhetesPorUsuario;
using App_Bets.Domain.Enuns;
using App_Bets.Domain.IRepositorio;
using App_Bets.Domain.Modelos;
using App_Bets.Domain.Services;
using AutoMapper;
using Moq;
using Xunit;

namespace App_Bets.Tests.Application.Queries.QueriesBilhetes
{
    public class BilhetesPorUsuarioHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IBilheteRepository> _bilheteRepositorioMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUsuarioContext> _usuarioContextMock;

        private readonly BilhetesPorUsuarioHandler _handler;

        public BilhetesPorUsuarioHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _bilheteRepositorioMock = new Mock<IBilheteRepository>();
            _mapperMock = new Mock<IMapper>();
            _usuarioContextMock = new Mock<IUsuarioContext>();

            _unitOfWorkMock
                .Setup(x => x.BilheteRepositorio)
                .Returns(_bilheteRepositorioMock.Object);

            _handler = new BilhetesPorUsuarioHandler(
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _usuarioContextMock.Object
            );
        }

        [Fact]
        public async Task Handle_DeveRetornarErro_QuandoNaoExistiremBilhetes()
        {
            // Arrange
            var query = new BilhetesPorUsuarioQuery(pageNumber: 1, pageSize: 10, MercadoEnum.Cartoes);
            var email = "fabio@email.com";

            _usuarioContextMock.Setup(x => x.Email).Returns(email);

            _bilheteRepositorioMock
                .Setup(x => x.GetBilhetesPorUsuario(email, MercadoEnum.Cartoes, query.PageNumber, query.PageSize))
                .ReturnsAsync((new List<Bilhete>(), 0));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Nenhum bilhete encontrado para este usuário.", result.Message);

            _mapperMock.Verify(
                x => x.Map<List<BilhetesListaPorUsuario>>(It.IsAny<List<Bilhete>>()),
                Times.Never);

            _bilheteRepositorioMock.Verify(
                x => x.GetBilhetesPorUsuario(email, MercadoEnum.Cartoes, query.PageNumber, query.PageSize),
                Times.Once);
        }

        [Fact]
        public async Task Handle_DeveRetornarSucesso_QuandoExistiremBilhetes()
        {
            // Arrange
            var query = new BilhetesPorUsuarioQuery(pageNumber: 1, pageSize: 10, MercadoEnum.Cartoes);
            var email = "fabio@email.com";

            var bilhetes = new List<Bilhete>
            {
                CriarBilhete(StatusEnum.Ganha, 2.0, 100),
                CriarBilhete(StatusEnum.Perdida, 1.8, 50)
            };

            var bilhetesDto = new List<BilhetesListaPorUsuario>
            {
                CriarBilheteDto(StatusEnum.Ganha, 2.0, 100),
                CriarBilheteDto(StatusEnum.Perdida, 1.8, 50)
            };

            var totalCount = 2;

            _usuarioContextMock.Setup(x => x.Email).Returns(email);

            _bilheteRepositorioMock
                .Setup(x => x.GetBilhetesPorUsuario(email, MercadoEnum.Cartoes, query.PageNumber, query.PageSize))
                .ReturnsAsync((bilhetes, totalCount));

            _mapperMock
                .Setup(x => x.Map<List<BilhetesListaPorUsuario>>(bilhetes))
                .Returns(bilhetesDto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data.Count);
            Assert.Equal(totalCount, result.TotalPage);

            Assert.Equal(bilhetesDto[0].UsuarioNome, result.Data[0].UsuarioNome);
            Assert.Equal(bilhetesDto[0].Status, result.Data[0].Status);
            Assert.Equal(bilhetesDto[1].UsuarioNome, result.Data[1].UsuarioNome);
            Assert.Equal(bilhetesDto[1].Status, result.Data[1].Status);

            _bilheteRepositorioMock.Verify(
                x => x.GetBilhetesPorUsuario(email, MercadoEnum.Cartoes, query.PageNumber, query.PageSize),
                Times.Once);

            _mapperMock.Verify(
                x => x.Map<List<BilhetesListaPorUsuario>>(bilhetes),
                Times.Once);
        }

        [Fact]
        public async Task Handle_DeveChamarRepositorioComEmailDoUsuarioLogado()
        {
            // Arrange
            var query = new BilhetesPorUsuarioQuery(pageNumber: 2, pageSize: 5, MercadoEnum.Gols);
            var email = "usuario@teste.com";

            var bilhetes = new List<Bilhete>
            {
                CriarBilhete(StatusEnum.Pendente, 1.5, 20)
            };

                    var bilhetesDto = new List<BilhetesListaPorUsuario>
            {
                CriarBilheteDto(StatusEnum.Pendente, 1.5, 20)
            };

            _usuarioContextMock.Setup(x => x.Email).Returns(email);

            _bilheteRepositorioMock
                .Setup(x => x.GetBilhetesPorUsuario(email, MercadoEnum.Gols, query.PageNumber, query.PageSize))
                .ReturnsAsync((bilhetes, 1));

            _mapperMock
                .Setup(x => x.Map<List<BilhetesListaPorUsuario>>(bilhetes))
                .Returns(bilhetesDto);

            // Act
            await _handler.Handle(query, CancellationToken.None);

            // Assert
            _bilheteRepositorioMock.Verify(
                x => x.GetBilhetesPorUsuario(
                    It.Is<string>(e => e == email),
                    It.Is<MercadoEnum?>(m => m == MercadoEnum.Gols),
                    It.Is<int>(p => p == 2),
                    It.Is<int>(s => s == 5)),
                Times.Once);
        }

        [Fact]
        public async Task Handle_DeveRetornarTotalCount_QuandoExistiremBilhetes()
        {
            // Arrange
            var query = new BilhetesPorUsuarioQuery(pageNumber: 1, pageSize: 10, MercadoEnum.Gols);
            var email = "fabio@email.com";

            var bilhetes = new List<Bilhete>
            {
                CriarBilhete(StatusEnum.Ganha, 2.0, 100)
            };

                    var bilhetesDto = new List<BilhetesListaPorUsuario>
            {
                CriarBilheteDto(StatusEnum.Ganha, 2.0, 100)
            };

            var totalCount = 37;

            _usuarioContextMock.Setup(x => x.Email).Returns(email);

            _bilheteRepositorioMock
                .Setup(x => x.GetBilhetesPorUsuario(email, MercadoEnum.Gols, query.PageNumber, query.PageSize))
                .ReturnsAsync((bilhetes, totalCount));

            _mapperMock
                .Setup(x => x.Map<List<BilhetesListaPorUsuario>>(bilhetes))
                .Returns(bilhetesDto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(totalCount, result.TotalPage);
        }

        private static Bilhete CriarBilhete(StatusEnum status, double odd, double valorApostado)
        {
            return new Bilhete(
                odd,
                valorApostado,
                TipoBanca.Bingo,
                status,
                MercadoEnum.Cartoes
            );
        }

        private static BilhetesListaPorUsuario CriarBilheteDto(StatusEnum status, double odd, double valorApostado)
        {
            return new BilhetesListaPorUsuarioBuilder()
                .ComUsuarioNome("Fabio")
                .ComStatus(status)
                .ComTipoBanca(TipoBanca.Bingo)
                .ComCasaAposta(CasaAposta.Betano)
                .Build();
        }
    }

    internal class BilhetesListaPorUsuarioBuilder
    {
        private readonly BilhetesListaPorUsuario _dto;

        public BilhetesListaPorUsuarioBuilder()
        {
            _dto = (BilhetesListaPorUsuario)Activator.CreateInstance(typeof(BilhetesListaPorUsuario), nonPublic: true)!;
        }

        public BilhetesListaPorUsuarioBuilder ComUsuarioNome(string nome)
        {
            _dto.UsuarioNome = nome;
            return this;
        }

        public BilhetesListaPorUsuarioBuilder ComStatus(StatusEnum status)
        {
            _dto.Status = status;
            return this;
        }

        public BilhetesListaPorUsuarioBuilder ComTipoBanca(TipoBanca tipoBanca)
        {
            _dto.TipoBanca = tipoBanca;
            return this;
        }

        public BilhetesListaPorUsuarioBuilder ComCasaAposta(CasaAposta casaAposta)
        {
            _dto.CasaAposta = casaAposta;
            return this;
        }

        public BilhetesListaPorUsuario Build()
        {
            return _dto;
        }
    }
}