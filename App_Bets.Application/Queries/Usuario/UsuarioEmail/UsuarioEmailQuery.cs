using App_Bets.Application.Dtos;
using App_Bets.Application.Dtos.Usuarios;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Application.Queries.Usuario.UsuarioEmail
{
    public class UsuarioEmailQuery : IRequest<ResultViewModel<UsuarioDetalhado>>
    {
    }
}
