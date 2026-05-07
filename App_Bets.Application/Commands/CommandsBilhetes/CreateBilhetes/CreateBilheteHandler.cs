using App_Bets.Application.Dtos;
using App_Bets.Domain.Enuns;
using App_Bets.Domain.IRepositorio;
using App_Bets.Domain.Modelos;
using App_Bets.Domain.Services;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace App_Bets.Application.Commands.CommandsBilhetes.CreateBilhetes
{
    public class CreateBilheteHandler : IRequestHandler<CreateBilheteCommand, ResultViewModel<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateBilheteHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IUsuarioContext _usuarioContext;
        private readonly IImagemStorageService _imagemStorageService;

        public CreateBilheteHandler(
            IUnitOfWork unitOfWork,
            ILogger<CreateBilheteHandler> logger,
            IMapper mapper,
            IUsuarioContext usuarioContext,
            IImagemStorageService imagemStorageService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _usuarioContext = usuarioContext;
            _imagemStorageService = imagemStorageService;
        }

        public async Task<ResultViewModel<Guid>> Handle(
            CreateBilheteCommand request,
            CancellationToken cancellationToken)
        {
            var userId = _usuarioContext.UserId;

            if (string.IsNullOrWhiteSpace(userId))
                return ResultViewModel<Guid>.Error("Usuário não autenticado");

            if (!Guid.TryParse(userId, out var usuarioId))
                return ResultViewModel<Guid>.Error("Id do usuário inválido");

            var usuario = await _unitOfWork.UsuarioRepositorio.GetById(usuarioId);

            if (usuario is null)
                return ResultViewModel<Guid>.Error("Usuário não encontrado");

            string? imagemUrl = null;

            if (request.Imagem is not null)
            {
                var resultadoValidacao = ValidarImagem(request.Imagem);

                if (!resultadoValidacao.IsValida)
                    return ResultViewModel<Guid>.Error(resultadoValidacao.Mensagem);

                var extensao = Path.GetExtension(request.Imagem.FileName);

                var nomeArquivo = $"{usuarioId}/{Guid.NewGuid()}{extensao}";

                await using var stream = request.Imagem.OpenReadStream();

                imagemUrl = await _imagemStorageService.UploadImagemAsync(
                    stream,
                    nomeArquivo,
                    request.Imagem.ContentType,
                    cancellationToken);
            }

            var dataAposta = request.DataAposta?.ToUniversalTime() ?? DateTime.UtcNow;

            var bilhete = new Bilhete(
                request.Odd,
                request.ValorApostado,
                request.TipoBanca,
                request.StatusEnum,
                request.Mercado,
                dataAposta,
                imagemUrl)
            {
                UsuarioId = usuarioId
            };

            bilhete.AtualizarCasaAposta(request.CasaAposta);

            var lucro = bilhete.ValorRetornado - bilhete.ValorApostado;

            if (bilhete.Status == StatusEnum.Ganha)
            {
                usuario.CreditarGanho(lucro);
            }
            else if (bilhete.Status == StatusEnum.Perdida)
            {
                usuario.DebitarPerda(bilhete.ValorApostado);
            }

            await _unitOfWork.UsuarioRepositorio.Update(usuario);
            await _unitOfWork.BilheteRepositorio.Add(bilhete);
            await _unitOfWork.Commit();

            _logger.LogInformation(
                "Bilhete criado com sucesso. BilheteId: {BilheteId}, UsuarioId: {UsuarioId}",
                bilhete.Id,
                usuarioId);

            return ResultViewModel<Guid>.Success(bilhete.Id);
        }

        private static (bool IsValida, string Mensagem) ValidarImagem(IFormFile imagem)
        {
            var tiposPermitidos = new[]
            {
                "image/jpeg",
                "image/png",
                "image/webp"
            };

            if (!tiposPermitidos.Contains(imagem.ContentType))
                return (false, "Formato de imagem inválido. Use JPG, PNG ou WEBP.");

            var tamanhoMaximo = 5 * 1024 * 1024;

            if (imagem.Length > tamanhoMaximo)
                return (false, "A imagem não pode ter mais que 5MB.");

            return (true, string.Empty);
        }
    }
}