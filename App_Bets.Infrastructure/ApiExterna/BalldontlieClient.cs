using App_Bet.Analytics.Interfaces;
using App_Bet.Analytics.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Infrastructure.ApiExterna
{
    public class BalldontlieClient : IBalldontlieClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BalldontlieClient> _logger;
        private readonly IMemoryCache _cache;

        public BalldontlieClient(
            HttpClient httpClient,
            IOptions<BalldontlieSettings> settings,
            ILogger<BalldontlieClient> logger,
            IMemoryCache cache)
        {
            _httpClient = httpClient;
            var config = settings.Value;

            _httpClient.BaseAddress = new Uri(config.BaseUrl);
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer",
                config.ApiKey);
            _logger = logger;
            _cache = cache;
        }

        public async Task<DetalhesJogador?> BuscarPlayer(string nome)
        {

            nome = Uri.UnescapeDataString(nome);
            Console.WriteLine(_httpClient.DefaultRequestHeaders.Authorization);

            // 🔥 Agora o cache guarda o objeto inteiro
            if (_cache.TryGetValue(nome, out DetalhesJogador jogadorCache))
            {
                _logger.LogInformation("Dados vieram do CACHE para {Nome}", nome);
                return jogadorCache;
            }


            _logger.LogInformation("Chamando API externa para {Nome}", nome);


            var partes = nome.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var firstName = partes.FirstOrDefault();
            var lastName = partes.Length > 1 ? partes.Last() : null;

            var url = lastName == null
                ? $"players?first_name={firstName}"
                : $"players?first_name={firstName}&last_name={lastName}";

            Console.WriteLine($"URL gerada: {url}");

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadFromJsonAsync<PlayerResponse>();

            var player = content?.data?.FirstOrDefault();

            if (player == null)
                return null;

            var detalhes = new DetalhesJogador
            {
                Id = player.id,
                NomeCompleto = $"{player.first_name} {player.last_name}",
                Posicao = TraduzirPosicao(player.position),
                Altura = ConverterAltura(player.height),
                Peso = ConverterPeso(player.weight),
                Camisa = player.jersey_number ?? "N/A",
                Faculdade = player.college ?? "N/A",
                Pais = player.country ?? "N/A",
                DraftInfo = player.draft_year != null
                    ? $"Ano {player.draft_year} | Round {player.draft_round} | Pick {player.draft_number}"
                    : "Undrafted",
                TimeAtual = player.team?.full_name ?? "Sem time"
            };

            Console.WriteLine($"Jogador encontrado: {detalhes.NomeCompleto}");

            // 🔥 Cache agora guarda o objeto inteiro
            _cache.Set(nome, detalhes, TimeSpan.FromMinutes(10));

            return detalhes;
        }

        private string ConverterAltura(string? alturaApi)
        {
            if (string.IsNullOrEmpty(alturaApi))
                return "N/A";

            var partes = alturaApi.Split('-');

            if (partes.Length != 2)
                return "N/A";

            if (!int.TryParse(partes[0], out int feet))
                return "N/A";

            if (!int.TryParse(partes[1], out int inches))
                return "N/A";

            double totalCm = (feet * 30.48) + (inches * 2.54);

            return $"{Math.Round(totalCm)} cm";
        }

        private string ConverterPeso(string? pesoApi)
        {
            if (string.IsNullOrEmpty(pesoApi))
                return "N/A";

            if (!double.TryParse(pesoApi, out double libras))
                return "N/A";

            double kg = libras * 0.453592;

            return $"{Math.Round(kg, 1)} kg";
        }

        private string TraduzirPosicao(string? posicao)
        {
            if (string.IsNullOrEmpty(posicao))
                return "Não informada";

            return posicao.ToUpper() switch
            {
                "PG" => "Armador",
                "SG" => "Ala-Armador",
                "SF" => "Ala",
                "PF" => "Ala-Pivô",
                "C" => "Pivô",
                "G" => "Armador",
                "F" => "Ala",
                _ => posicao
            };
        }

        #region//public async Task<int?> BuscarPlayerId(string nome)
        //{
        //    if (_cache.TryGetValue(nome, out int? playerId))
        //        return playerId;

        //    var partes = nome.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        //    var firstName = partes.FirstOrDefault();
        //    var lastName = partes.Length > 1 ? partes.Last() : null;

        //    var url = lastName == null
        //        ? $"players?first_name={firstName}"
        //        : $"players?first_name={firstName}&last_name={lastName}";

        //    Console.WriteLine($"URL gerada: {url}");

        //    var response = await _httpClient.GetAsync(url);

        //    if (!response.IsSuccessStatusCode)
        //    {
        //        Console.WriteLine($"Erro HTTP: {response.StatusCode}");
        //        return null;
        //    }

        //    // 🔥 AQUI ESTÁ O TESTE IMPORTANTE
        //    var jsonBruto = await response.Content.ReadAsStringAsync();

        //    Console.WriteLine("JSON retornado pela API:");
        //    Console.WriteLine(jsonBruto);

        //    return null; // só para teste
        //}
        #endregion




    }
}
