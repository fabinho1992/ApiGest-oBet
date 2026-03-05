using App_Bets.Domain.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Infrastructure.Services.AuthService
{
    public class UsuarioContext : IUsuarioContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsuarioContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Email =>
            _httpContextAccessor.HttpContext?
                .User.FindFirst(ClaimTypes.Email)?.Value;

        public string UserId =>
            _httpContextAccessor.HttpContext?
                .User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}
