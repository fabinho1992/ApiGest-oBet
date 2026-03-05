using App_Bets.Domain.IRepositorio;
using App_Bets.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private IUsuarioRepositorio? _usuarioRepositorio;
        private IBilheteRepository? _bilheteRepositorio;

        private readonly BetDbContext _context;

        public UnitOfWork(BetDbContext context)
        {
            _context = context;
        }

        public IUsuarioRepositorio UsuarioRepositorio
        {
            get
            {
                return _usuarioRepositorio = _usuarioRepositorio ?? new UsuarioRepository(_context);
            }
        }

        public IBilheteRepository BilheteRepositorio
        {
            get
            {
                return _bilheteRepositorio = _bilheteRepositorio ?? new BilheteRepository(_context);
            }
        }

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }

    }
}
