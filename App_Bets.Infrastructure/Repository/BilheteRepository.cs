using App_Bets.Domain.Enuns;
using App_Bets.Domain.IRepositorio;
using App_Bets.Domain.Modelos;
using App_Bets.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace App_Bets.Infrastructure.Repository
{
    public class BilheteRepository : RepositoryBase<Bilhete>, IBilheteRepository
    {
        private readonly BetDbContext _context;

        public BilheteRepository(BetDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<(List<Bilhete> bilhetes, int totalPaginas, int totalCount)> GetBilhetesPorCasaAposta(string email, CasaAposta casaAposta, MercadoEnum? mercado, int pageNumer, int pageSize)
        {
            var query = _context.Bilhetes
                .Include(b => b.Usuario)
                .Where(b =>
                    b.Usuario.Email == email &&
                    b.CasaAposta == casaAposta);


            if (mercado.HasValue)
            {
                query = query.Where(b => b.Mercado == mercado.Value);
            }

            var totalCount = query.Count();

            var items = await query
                .OrderByDescending(b => b.DataAposta)
                .Skip((pageNumer - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalPaginas = (int)Math.Ceiling(
                (double)totalCount / pageSize
                    );

            return (items, totalPaginas, totalCount);


        }

        public async Task<(List<Bilhete> bilhetes, int totalPaginas)> GetBilhetesPorData(
         string email,
         DateTime data,
         MercadoEnum? mercado,
         int pageNumer,
         int pageSize)
        {
            TimeZoneInfo tz;

            try
            {
                tz = TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");
            }
            catch
            {
                tz = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            }

            var dataBrasilInicio = DateTime.SpecifyKind(data.Date, DateTimeKind.Unspecified);
            var dataBrasilFim = dataBrasilInicio.AddDays(1);

            var dataInicioUtc = TimeZoneInfo.ConvertTimeToUtc(dataBrasilInicio, tz);
            var dataFimUtc = TimeZoneInfo.ConvertTimeToUtc(dataBrasilFim, tz);

            var query = _context.Bilhetes
                .Include(b => b.Usuario)
                .Where(b =>
                    b.Usuario.Email == email &&
                    b.DataAposta >= dataInicioUtc &&
                    b.DataAposta < dataFimUtc);

            if (mercado.HasValue)
            {
                query = query.Where(b => b.Mercado == mercado.Value);
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(b => b.DataAposta)
                .Skip((pageNumer - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalPaginas = (int)Math.Ceiling((double)totalCount / pageSize);

            return (items, totalPaginas);
        }

        public async Task<(List<Bilhete> bilhetes, int totalPaginas)> GetBilhetesPorUsuario(
             string email,
             MercadoEnum? mercadoEnum,
             int pageNumber, int pageSize)
                {
                    var bilhetesQuery = _context.Bilhetes
                        .Include(b => b.Usuario)
                        .Where(b => b.Usuario.Email == email);

                    if (mercadoEnum.HasValue)
                    {
                        bilhetesQuery = bilhetesQuery
                            .Where(b => b.Mercado == mercadoEnum.Value);
                    }

                    var totalCount = await bilhetesQuery.CountAsync();

                    var bilhetes = await bilhetesQuery
                        .OrderByDescending(b => b.DataAposta)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync();

                                var totalPaginas = (int)Math.Ceiling(
                    (double)totalCount / pageSize
                    );

            return (bilhetes, totalPaginas);
        }

        public async Task<Dashboard> GetDashboard(string email)
        {
            var bilhetes = _context.Bilhetes
                .Where(b => b.Usuario.Email == email);

            var totalGanhas = await bilhetes
                .CountAsync(b => b.Status == StatusEnum.Ganha);

            var totalPerdidas = await bilhetes
                .CountAsync(b => b.Status == StatusEnum.Perdida);

            var lucroTotal = await bilhetes
                .Where(b => b.Status == StatusEnum.Ganha)
                .SumAsync(b => b.ValorRetornado - b.ValorApostado);

            var totalInvestido = await bilhetes
                .SumAsync(b => b.ValorApostado);

            var prejuizoTotal = await bilhetes
                .Where(b => b.Status == StatusEnum.Perdida)
                .SumAsync(b => b.ValorApostado);

            return new Dashboard
            {
                TotalGanhas = totalGanhas,
                TotalPerdidas = totalPerdidas,
                Lucro = lucroTotal,
                TotalInvestido = totalInvestido,
                Prejuizo = prejuizoTotal
            };
        }

        public async Task<List<CasaApostaResumo>> GetResumoCasas(string email, MercadoEnum? mercado)
        {
            var query = _context.Bilhetes
                .Where(b => b.Usuario.Email == email);

            if (mercado.HasValue)
                query = query.Where(b => b.Mercado == mercado.Value);

            return await query
                .GroupBy(b => b.CasaAposta)
                .Select(g => new CasaApostaResumo
                {
                    CasaAposta = g.Key,
                    Quantidade = g.Count()
                })
                .ToListAsync();
        }


        public async Task DeleteAll(string email)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
            var bilhetes = _context.Bilhetes.Where(b => b.UsuarioId == usuario.Id);

            _context.Bilhetes.RemoveRange(bilhetes);

        }

        public async Task<(List<Bilhete> bilhetes, int totalPaginas)> GetBilhetesPorStatus(string email, StatusEnum status, MercadoEnum? mercado, int pageNumer, int pageSize)
        {
            var query = _context.Bilhetes.Include(b => b.Usuario)
                .Where(b => b.Usuario.Email == email && b.Status == status);


            if (mercado.HasValue)
            {
                query = query.Where(b => b.Mercado == mercado.Value);
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pageNumer - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalPaginas = (int)Math.Ceiling(
                (double)totalCount / pageSize
                    );

            return (items, totalPaginas);

        }

        public async Task<(List<Bilhete> bilhetes, int totalPaginas, int totalCount)> GetBilhetesPorMercado(string email, MercadoEnum? mercado, int pageNumer, int pageSize)
        {
            var query = _context.Bilhetes
                .Include(b => b.Usuario)
                .Where(b =>
                    b.Usuario.Email == email &&
                    b.Mercado == mercado);

            var totalCount = query.Count();

            var items = await query
                .OrderByDescending(b => b.DataAposta)
                .Skip((pageNumer - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalPaginas = (int)Math.Ceiling(
                (double)totalCount / pageSize
                    );

            return (items, totalPaginas, totalCount);
        }

        public async Task<Dashboard> GetDashboardMercado(string email, MercadoEnum? mercado)
        {

            var bilhetes = _context.Bilhetes
                .Where(b => b.Usuario.Email == email);

            if (mercado.HasValue)
            {
                bilhetes = bilhetes.Where(b => b.Mercado == mercado.Value);
            }

            var totalGanhas = await bilhetes
                .CountAsync(b => b.Status == StatusEnum.Ganha);

            var totalPerdidas = await bilhetes
                .CountAsync(b => b.Status == StatusEnum.Perdida);

            var totalPendentes = await bilhetes
                .CountAsync(b => b.Status == StatusEnum.Pendente);

            var lucroTotal = await bilhetes
                .Where(b => b.Status == StatusEnum.Ganha)
                .SumAsync(b => b.ValorRetornado - b.ValorApostado);

            var totalInvestido = await bilhetes
                .SumAsync(b => b.ValorApostado);

            var prejuizoTotal = await bilhetes
                .Where(b => b.Status == StatusEnum.Perdida)
                .SumAsync(b => b.ValorApostado);

            return new Dashboard
            {
                TotalGanhas = totalGanhas,
                TotalPerdidas = totalPerdidas,
                TotalPendentes = totalPendentes,
                Lucro = lucroTotal,
                TotalInvestido = totalInvestido,
                Prejuizo = prejuizoTotal
            };
        }

        public async Task<(List<Bilhete> Bilhetes, int TotalPaginas)> ObterBilhetesFiltradosPorUsuarioAsync(
    string email,
    int pageNumber,
    int pageSize,
    CasaAposta? casaAposta,
    MercadoEnum? mercado,
    StatusEnum? status,
    DateTime? data)
        {
            var query = _context.Bilhetes
                .AsNoTracking()
                .Include(b => b.Usuario)
                .Where(b => b.Usuario.Email == email);

            if (casaAposta.HasValue)
            {
                query = query.Where(b => b.CasaAposta == casaAposta.Value);
            }

            if (mercado.HasValue)
            {
                query = query.Where(b => b.Mercado == mercado.Value);
            }

            if (status.HasValue)
            {
                query = query.Where(b => b.Status == status.Value);
            }

            if (data.HasValue)
            {
                TimeZoneInfo tz;

                try
                {
                    tz = TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");
                }
                catch
                {
                    tz = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
                }

                var inicioBrasil = DateTime.SpecifyKind(data.Value.Date, DateTimeKind.Unspecified);
                var fimBrasil = inicioBrasil.AddDays(1);

                var inicioUtc = TimeZoneInfo.ConvertTimeToUtc(inicioBrasil, tz);
                var fimUtc = TimeZoneInfo.ConvertTimeToUtc(fimBrasil, tz);

                query = query.Where(b => b.DataAposta >= inicioUtc && b.DataAposta < fimUtc);
            }

            var totalRegistros = await query.CountAsync();

            var totalPaginas = (int)Math.Ceiling(totalRegistros / (double)pageSize);

            var bilhetes = await query
                .OrderByDescending(b => b.DataAposta)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (bilhetes, totalPaginas);
        }

        public async Task<List<Bilhete>> ObterBilhetesFiltradosParaRelatorioAsync(
             string email,
             CasaAposta? casaAposta,
             MercadoEnum? mercado,
             StatusEnum? status,
             DateTime? data,
             int pageNumber,
             int pageSize,
             bool somentePaginaAtual)
        {
            var query = _context.Bilhetes
                .AsNoTracking()
                .Include(b => b.Usuario)
                .Where(b => b.Usuario.Email == email);

            if (casaAposta.HasValue)
                query = query.Where(b => b.CasaAposta == casaAposta.Value);

            if (mercado.HasValue)
                query = query.Where(b => b.Mercado == mercado.Value);

            if (status.HasValue)
                query = query.Where(b => b.Status == status.Value);

            if (data.HasValue)
            {
                TimeZoneInfo tz;

                try
                {
                    tz = TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");
                }
                catch
                {
                    tz = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
                }

                var inicioBrasil = DateTime.SpecifyKind(data.Value.Date, DateTimeKind.Unspecified);
                var fimBrasil = inicioBrasil.AddDays(1);

                var inicioUtc = TimeZoneInfo.ConvertTimeToUtc(inicioBrasil, tz);
                var fimUtc = TimeZoneInfo.ConvertTimeToUtc(fimBrasil, tz);

                query = query.Where(b => b.DataAposta >= inicioUtc && b.DataAposta < fimUtc);
            }

            query = query.OrderByDescending(b => b.DataAposta);

            if (somentePaginaAtual)
            {
                query = query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);
            }

            return await query.ToListAsync();
        }
    }
}

