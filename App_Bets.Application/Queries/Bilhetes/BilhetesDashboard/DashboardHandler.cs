using App_Bets.Application.Dtos;
using App_Bets.Application.Dtos.Bilhetes;
using App_Bets.Domain.IRepositorio;
using App_Bets.Domain.Modelos;
using App_Bets.Domain.Services;
using MediatR;

namespace App_Bets.Application.Queries.Bilhetes.BilhetesDashboard
{
    public class DashboardHandler : IRequestHandler<DashboardQuery, ResultViewModel<DashboardDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUsuarioContext _usuarioContext;

        public DashboardHandler(IUnitOfWork unitOfWork, IUsuarioContext usuarioContext)
        {
            _unitOfWork = unitOfWork;
            _usuarioContext = usuarioContext;
        }

        public async Task<ResultViewModel<DashboardDto>> Handle(DashboardQuery request, CancellationToken cancellationToken)
        {
            var email = _usuarioContext.Email;

            var dashboard = await _unitOfWork.BilheteRepositorio.GetDashboard(email);
            if (dashboard is null)
            {
                return ResultViewModel<DashboardDto>.Error("Não foi possível obter os dados do dashboard.");
            }

            var totalInvestido = dashboard.TotalInvestido;

            var roi = totalInvestido == 0
                ? "0%"
                : $"{(dashboard.ResultadoFinal / totalInvestido) * 100:0.00}%";

            var dashboardRetorno = new DashboardDto
            {
                TotalGanhas = dashboard.TotalGanhas,
                TotalPerdidas = dashboard.TotalPerdidas,
                Lucro = dashboard.Lucro,
                Prejuizo = dashboard.Prejuizo,
                TotalInvestido = dashboard.TotalInvestido,
                ResultadoFinal = dashboard.ResultadoFinal,
                ROI = roi
            };

            return ResultViewModel<DashboardDto>.Success(dashboardRetorno);

        }
    }
}
