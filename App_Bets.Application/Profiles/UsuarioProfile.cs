using App_Bets.Application.Commands.CommandsUser.CreateUsuario;
using App_Bets.Application.Dtos.Usuarios;
using App_Bets.Domain.Modelos;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Application.Profiles
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            CreateMap<CreateUserCommand, Usuario>()
            .ConstructUsing(cmd =>
                new Usuario(
                    cmd.DisplayName,
                    cmd.Cpf,
                    cmd.Email,
                    cmd.BancaInicial,
                    cmd.MetaBanca
                ));

            CreateMap<Usuario, UsuarioDetalhado>();
        }
    }
}
