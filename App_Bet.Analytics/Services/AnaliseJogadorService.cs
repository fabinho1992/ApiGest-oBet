//using App_Bet.Analytics.Interfaces;
//using App_Bet.Analytics.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace App_Bet.Analytics.Services
//{
//    public class AnaliseJogadorService
//    {
//        private readonly IBalldontlieClient _client;

//        public AnaliseJogadorService(IBalldontlieClient client)
//        {
//            _client = client;
//        }

//        public async Task<EstatisticaJogador?> Analisar(string nome)
//        {
//            var playerId = await _client.BuscarPlayerId(nome);
//            if (playerId == null) return null;

//            var stats = await _client.BuscarStats(playerId.Value);
            
//                return new EstatisticaJogador
//                {
//                    Nome = nome,
//                    JogosConsiderados = 0
//                };
            

//            var mediaPontos = stats.Average(x => x.Pontos);
//            var mediaRebotes = stats.Average(x => x.Rebotes);
//            var mediaAssistencias = stats.Average(x => x.Assistencias);

//            int duploDuplo = stats.Count(s =>
//            {
//                int categorias = 0;
//                if (s.Pontos >= 10) categorias++;
//                if (s.Rebotes >= 10) categorias++;
//                if (s.Assistencias >= 10) categorias++;
//                return categorias >= 2;
//            });

//            return new EstatisticaJogador
//            {
//                Nome = nome,
//                MediaPontos = Math.Round(mediaPontos, 2),
//                MediaRebotes = Math.Round(mediaRebotes, 2),
//                MediaAssistencias = Math.Round(mediaAssistencias, 2),
//                JogosConsiderados = stats.Count,
//                TotalDuploDuplo = duploDuplo,
//                PercentualDuploDuplo = Math.Round((double)duploDuplo / stats.Count * 100, 2)
//            };
//        }
//    }
//}
