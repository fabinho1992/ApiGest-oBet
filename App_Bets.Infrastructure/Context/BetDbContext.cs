using App_Bets.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace App_Bets.Infrastructure.Context;

public class BetDbContext : IdentityDbContext<ApplicationUser>
{
    public BetDbContext(DbContextOptions<BetDbContext> options)
          : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Bilhete> Bilhetes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Aplica mappings
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

}


