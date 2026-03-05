using App_Bets.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Infrastructure.Configuracao
{
    public class UsuarioConfiguracao : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {

            builder.ToTable("Usuarios");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.DisplayName)
                .HasMaxLength(256);

            builder.Property(u => u.Email)
                .HasMaxLength(256);

            builder.Property(u => u.DisplayName)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(u => u.Cpf)
                .IsRequired()
                .HasMaxLength(11);

            builder.HasIndex(u => u.Cpf)
                .IsUnique();

            builder.Property(u => u.BancaInicial)
                .HasColumnType("decimal(18,2)");

            builder.Property(u => u.BancaAtual)
                .HasColumnType("decimal(18,2)");

            builder.Property(u => u.MetaBanca)
                .HasColumnType("decimal(18,2)");

            // Data
            builder.Property(u => u.DataCriacao)
                .IsRequired();

            builder.HasOne<ApplicationUser>()
            .WithOne(a => a.Usuario)
            .HasForeignKey<ApplicationUser>(a => a.UsuarioId);

            builder.HasIndex(u => u.Email).IsUnique();
        }
    }
}
