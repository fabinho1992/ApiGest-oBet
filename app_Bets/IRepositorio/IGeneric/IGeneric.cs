using App_Bets.Domain.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Domain.IRepositorio.IGeneric
{
    public interface IGeneric<T> where T : class
    {
        Task Add(T entity);
        Task<T> GetById(Guid id);
        Task Update(T entity);
        Task Delete(Guid id);
        Task<(IEnumerable<T> items, int totalCount)> GetAll(ParametrosPaginacao parametrosPaginacao);
    }
}
