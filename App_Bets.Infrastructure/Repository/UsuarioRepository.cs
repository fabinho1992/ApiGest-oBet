using App_Bets.Domain.IRepositorio;
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
    public class UsuarioRepository : RepositoryBase<Usuario>, IUsuarioRepositorio
    {
        private readonly BetDbContext _context;

        public UsuarioRepository(BetDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Usuario> GetUsuaioEmail(string email)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(p => p.Email == email);

            return usuario;
        }

        public async Task<Usuario> GetUsuarioCpf(string cpf)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Bilhetes)
                .FirstOrDefaultAsync(p => p.Cpf == cpf); 
            
            return usuario;
        }
    }
}
