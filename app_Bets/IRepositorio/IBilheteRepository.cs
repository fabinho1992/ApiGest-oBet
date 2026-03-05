using App_Bets.Domain.IRepositorio.IGeneric;
using App_Bets.Domain.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Domain.IRepositorio
{
    public interface IBilheteRepository : IGeneric<Bilhete>
    {
        Task<(List<Bilhete> bilhetes, int totalPaginas)> GetBilhetesPorUsuario(string email, ParametrosPaginacao parametrosPaginacao);
        Task<(List<Bilhete> bilhetes, int totalPaginas)> GetBilhetesPorData(string email, DateTime data, int pageNumer, int pageSize);
        Task<Dashboard> GetDashboard(string email);


    }
}
