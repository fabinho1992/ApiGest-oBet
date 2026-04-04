using App_Bets.Domain.Enuns;
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
        Task<(List<Bilhete> bilhetes, int totalPaginas)> GetBilhetesPorUsuario(string email, MercadoEnum? mercadoEnum, int pageNumber, int pageSize);
        Task<(List<Bilhete> bilhetes, int totalPaginas)> GetBilhetesPorData(string email, DateTime data, MercadoEnum? mercado, int pageNumer, int pageSize);
        Task<(List<Bilhete> bilhetes, int totalPaginas, int totalCount)> GetBilhetesPorCasaAposta(string email, CasaAposta casaAposta, MercadoEnum? mercado, int pageNumer, int pageSize);
        Task<(List<Bilhete> bilhetes, int totalPaginas)> GetBilhetesPorStatus(string email, StatusEnum status, MercadoEnum? mercado, int pageNumer, int pageSize);
        Task<List<CasaApostaResumo>> GetResumoCasas(string email, MercadoEnum? mercado);
        Task DeleteAll(string email);
        Task<Dashboard> GetDashboard(string email);
        Task<(List<Bilhete> bilhetes, int totalPaginas, int totalCount)> GetBilhetesPorMercado(string email, MercadoEnum? casaAposta, int pageNumer, int pageSize);
        Task<Dashboard> GetDashboardMercado(string email, MercadoEnum? mercado);
        Task<(List<Bilhete> Bilhetes, int TotalPaginas)> ObterBilhetesFiltradosPorUsuarioAsync(
            string email,
            int pageNumber,
            int pageSize,
            CasaAposta? casaAposta,
            MercadoEnum? mercado,
            StatusEnum? status,
            DateTime? data);

    }
}
