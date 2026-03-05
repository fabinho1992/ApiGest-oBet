using App_Bets.Application.Dtos;
using App_Bets.Application.Dtos.Bilhetes;
using App_Bets.Application.Queries.Bilhetes;
using App_Bets.Application.Queries.Bilhetes.BilhetesPorUsuario;
using App_Bets.Domain.IRepositorio;
using App_Bets.Domain.Services;
using AutoMapper;
using MediatR;

public class BilhetesPorUsuarioHandler
    : IRequestHandler<BilhetesPorUsuarioQuery, ResultViewModel<List<BilhetesListaPorUsuario>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUsuarioContext _usuarioContext;

    public BilhetesPorUsuarioHandler(IUnitOfWork unitOfWork, IMapper mapper, IUsuarioContext usuarioContext)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _usuarioContext = usuarioContext;
    }

    public async Task<ResultViewModel<List<BilhetesListaPorUsuario>>> Handle(
        BilhetesPorUsuarioQuery request,
        CancellationToken cancellationToken)
    {

        var email = _usuarioContext.Email;
        
        var (bilhetes, totalCount) =
            await _unitOfWork.BilheteRepositorio
                .GetBilhetesPorUsuario(email, request);

        if (bilhetes == null || !bilhetes.Any())
        {
            return ResultViewModel<List<BilhetesListaPorUsuario>>
                .Error("Nenhum bilhete encontrado para este usuário.");
        }

        var bilhetesDto =
            _mapper.Map<List<BilhetesListaPorUsuario>>(bilhetes);

        
        return ResultViewModel<List<BilhetesListaPorUsuario>>
            .Success(bilhetesDto, totalCount);
    }
}
