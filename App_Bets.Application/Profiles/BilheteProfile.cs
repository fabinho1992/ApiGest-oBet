using App_Bets.Application.Dtos.bilhetes;
using App_Bets.Application.Dtos.Bilhetes;
using App_Bets.Domain.Modelos;
using AutoMapper;

namespace App_Bets.Application.Profiles
{
    public class BilheteProfile : Profile
    {
        public BilheteProfile()
        {
            CreateMap<Bilhete, BilheteConsultaCpf>()
            .ForMember(
                dest => dest.Usuarioname,
                opt => opt.MapFrom(src => src.Usuario.DisplayName)
            )
            .ForMember(dest => dest.DataAposta, opt => opt.MapFrom(src => src.DataAposta)
            )
            .ReverseMap();

            CreateMap<Bilhete, BilheteLista>()
            .ForMember(
                dest => dest.DataAposta,
                opt => opt.MapFrom(src => src.DataAposta)
            )
            .ReverseMap();

            CreateMap<Bilhete, BilhetesListaPorUsuario>()
            .ForMember(dest => dest.UsuarioNome,
                opt => opt.MapFrom(src => src.Usuario.DisplayName));

            CreateMap<CasaApostaResumo, CasaApostaResumoDto>().ReverseMap();

        }
    }
}
