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
    public class BilheteConfiguracao : IEntityTypeConfiguration<Bilhete>
    {
        public void Configure(EntityTypeBuilder<Bilhete> builder)
        {
            builder.ToTable("Bilhetes");

            builder.HasKey(b => b.Id);

            builder.HasOne(b => b.Usuario)
                .WithMany(u => u.Bilhetes)
                .HasForeignKey(b => b.UsuarioId);

            // Odd
            builder.Property(b => b.Odd)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            // Valor apostado
            builder.Property(b => b.ValorApostado)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            // Valor retornado
            builder.Property(b => b.ValorRetornado)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            // Data da aposta
            builder.Property(b => b.DataAposta)
                .IsRequired();

            // Status enum
            builder.Property(b => b.Status)
                .HasConversion<string>()
                .IsRequired();

            // Tipo de banca enum
            builder.Property(b => b.TipoBanca)
                .HasConversion<string>()
               .IsRequired();
        }
    }
}
