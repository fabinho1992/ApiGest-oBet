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

namespace App_Bets.Infrastructure.Repository
{
    public class BilheteRepository : RepositoryBase<Bilhete>, IBilheteRepository
    {
        private readonly BetDbContext _context;

        public BilheteRepository(BetDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<(List<Bilhete> bilhetes, int totalPaginas, int totalCount)> GetBilhetesPorCasaAposta(string email, CasaAposta casaAposta, int pageNumer, int pageSize)
        {
            var query = _context.Bilhetes
                .Include(b => b.Usuario)
                .Where(b =>
                    b.Usuario.Email == email &&
                    b.CasaAposta == casaAposta);

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

        public async Task<(List<Bilhete> bilhetes, int totalPaginas)> GetBilhetesPorData(string email, DateTime data, int pageNumer, int pageSize)
        {
            var dataInicio = data.Date;
            var dataFim = dataInicio.AddDays(1);

            var query = _context.Bilhetes
                .Include(b => b.Usuario)
                .Where(b =>
                    b.Usuario.Email == email &&
                    b.DataAposta >= dataInicio &&
                    b.DataAposta < dataFim);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(b => b.DataAposta)
                .Skip((pageNumer - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalPaginas = (int)Math.Ceiling(
                (double)totalCount / pageSize
                    );

            return (items, totalPaginas);
        }

        public async Task<(List<Bilhete> bilhetes, int totalPaginas)> GetBilhetesPorUsuario(string email, ParametrosPaginacao parametrosPaginacao)
        {
            var query = _context.Bilhetes.Include(b => b.Usuario)
                .Where(b => b.Usuario.Email == email);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((parametrosPaginacao.PageNumber - 1) * parametrosPaginacao.PageSize)
                .Take(parametrosPaginacao.PageSize)
                .ToListAsync();

            var totalPaginas = (int)Math.Ceiling(
                (double)totalCount / parametrosPaginacao.PageSize
                    );

            return (items, totalPaginas);
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

        public async Task<List<CasaApostaResumo>> GetResumoCasas(string email)
        {
            return await _context.Bilhetes
                .Where(b => b.Usuario.Email == email)
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

        public async Task<(List<Bilhete> bilhetes, int totalPaginas)> GetBilhetesPorStatus(string email, StatusEnum status, int pageNumer, int pageSize)
        {
            var query = _context.Bilhetes.Include(b => b.Usuario)
                .Where(b => b.Usuario.Email == email && b.Status == status);

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
    }
}
