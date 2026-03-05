using App_Bets.Domain.IRepositorio.IGeneric;
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
    public class RepositoryBase<T> : IGeneric<T> where T : class
    {
        private readonly BetDbContext _context;

        public RepositoryBase(BetDbContext context)
        {
            _context = context;
        }

        public async Task Add(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public async Task Delete(Guid id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            _context.Set<T>().Remove(entity);
        }

        public async Task<(IEnumerable<T> items, int totalCount)> GetAll(ParametrosPaginacao parametrosPaginacao)
        {
            var query = _context.Set<T>().AsNoTracking();

            // Primeiro obtemos o total de registros
            var totalCount = await query.CountAsync();

            // Depois aplicamos a paginação
            var items = await query
                .Skip((parametrosPaginacao.PageNumber - 1) * parametrosPaginacao.PageSize)
                .Take(parametrosPaginacao.PageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<T> GetById(Guid id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            return entity;
        }

        public async Task Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
    }
}
