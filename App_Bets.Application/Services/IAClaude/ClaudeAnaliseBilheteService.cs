using Anthropic.SDK;
using Anthropic.SDK.Constants;
using Anthropic.SDK.Messaging;
using App_Bets.Application.Dtos.Bilhetes;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace App_Bets.Application.Services.IAClaude
{
    public class ClaudeAnaliseBilheteService : IClaudeAnaliseBilheteService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ClaudeAnaliseBilheteService> _logger;

        public ClaudeAnaliseBilheteService(
            IConfiguration configuration,
            ILogger<ClaudeAnaliseBilheteService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<BilheteExtraidoDto> AnalisarImagemAsync(
            IFormFile imagem,
            CancellationToken cancellationToken = default)
        {
            if (imagem is null || imagem.Length == 0)
                throw new ArgumentException("Imagem inválida ou vazia.", nameof(imagem));

            var apiKey = _configuration["Anthropic:ApiKey"]
                ?? throw new InvalidOperationException("ApiKey do Anthropic não configurada.");

            var base64Imagem = await ConverterImagemParaBase64(imagem, cancellationToken);

            using var client = new AnthropicClient(apiKey);

            var prompt = MontarPrompt();

            var parameters = new MessageParameters
            {
                Messages = new List<Message>
                {
                    new Message
                    {
                        Role = RoleType.User,
                        Content = new List<ContentBase>
                        {
                            new ImageContent
                            {
                                Source = new ImageSource
                                {
                                    MediaType = imagem.ContentType,
                                    Data = base64Imagem
                                }
                            },
                            new TextContent
                            {
                                Text = prompt
                            }
                        }
                    }
                },
                MaxTokens = 1024,
                Model = AnthropicModels.Claude45Sonnet,
                Stream = false,
                Temperature = 0.0m
            };

            var response = await client.Messages.GetClaudeMessageAsync(parameters, cancellationToken);

            var textoResposta = response.Content
                .OfType<TextContent>()
                .FirstOrDefault()?.Text;

            if (string.IsNullOrWhiteSpace(textoResposta))
            {
                _logger.LogWarning("Claude retornou resposta vazia para análise da imagem.");
                return new BilheteExtraidoDto();
            }

            _logger.LogInformation("Resposta bruta do Claude: {Resposta}", textoResposta);

            return DesserializarRespostaClaude(textoResposta);
        }

        private static async Task<string> ConverterImagemParaBase64(
            IFormFile imagem,
            CancellationToken cancellationToken)
        {
            using var memoryStream = new MemoryStream();
            await imagem.CopyToAsync(memoryStream, cancellationToken);
            return Convert.ToBase64String(memoryStream.ToArray());
        }

        private static string MontarPrompt()
        {
            return @"Analise esta imagem de um bilhete de aposta esportiva e extraia exatamente os seguintes campos.
                Responda APENAS com um JSON válido, sem texto adicional, sem markdown e sem crases.

                Estrutura esperada:
                {
                  ""OddsIndividuais"": [<lista com todas as odds individuais de cada seleção>],
                  ""Odd"": <odd total se estiver visível no bilhete, caso contrário null>,
                  ""ValorApostado"": <decimal ou null>,
                  ""Mercado"": <string ou null>,
                  ""CasaAposta"": <string ou null>,
                  ""DataAposta"": <string ISO 8601 (yyyy-MM-ddTHH:mm:ss) ou null>
                }

                Regras:
                - OddsIndividuais: liste o número decimal de CADA seleção da imagem, de cima para baixo.
                  Cada seleção tem um número decimal alinhado à direita do nome (ex: 1.42, 2.45).
                  ATENÇÃO: números inteiros seguidos de + (ex: 6+, 7+, 16+, 25+) são metas
                  estatísticas de jogadores e NÃO são odds. Ignore completamente esses valores.
                  Inclua TODAS as odds decimais, independente do layout ou formato do bloco.
                - Odd: use APENAS se houver um valor de odd total claramente destacado no bilhete.
                  Se um bloco de seleções tiver um número decimal destacado no cabeçalho (ex: 14.50),
                  esse valor É a odd total daquele bloco, use diretamente sem multiplicar nada.
                  Se não houver odd total visível, retorne null. NUNCA calcule, NUNCA divida retorno por aposta.
                - ValorApostado: valor do campo Aposta em R$. NUNCA use Retorno Total.
                - IGNORE: R$, retorno, prêmio, +15%, PAGAMENTO ANTECIPADO, Aumento.
                - Mercado: analise TODAS as seleções do bilhete e siga estas regras:
                    1. Se TODOS os jogos forem de basquete, use Basquete.
                    2. Se TODOS os jogos forem de futebol:
                       - Se a maioria das seleções for Resultado Final, use ResultadoFinal.
                       - Se todas as seleções forem Ambas Marcam, use AmbasMarcam.
                       - Se todas as seleções forem sobre gols (over/under), use Gols.
                       - Se misturar mercados de futebol diferentes, use Multipla.
                    3. Se misturar esportes diferentes (futebol + basquete, etc), use Multipla.
                    4. Use um destes valores: Escanteios, Gols, Cartoes, AmbasMarcam,
                       ResultadoFinal, Basquete, Multipla.
                - CasaAposta: use um destes valores: Betano, Bet365, SuperBet, SportingBet, EsportivaBet.
                - Se algum campo não puder ser identificado, retorne null.
                - Não inclua nenhum campo além dos listados.";
        }

        private static BilheteExtraidoDto DesserializarRespostaClaude(string textoResposta)
        {
            var jsonLimpo = ExtrairJson(textoResposta);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var dados = JsonSerializer.Deserialize<BilheteExtraidoDto>(jsonLimpo, options)
                ?? new BilheteExtraidoDto();

            // Se não veio odd total, calcula multiplicando as individuais
            if (dados.Odd is null && dados.OddsIndividuais?.Count > 0)
            {
                dados.Odd = Math.Round(
                    dados.OddsIndividuais.Aggregate(1.0, (acc, odd) => acc * odd),
                    2);
            }

            return dados;
        }

        private static string ExtrairJson(string texto)
        {
            var match = Regex.Match(texto, @"\{[\s\S]*\}");
            return match.Success ? match.Value : texto;
        }
    }
}
