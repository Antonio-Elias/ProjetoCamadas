﻿using CamadaBusiness.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CamadaData.Mappings
{

    public class ProdutoMapping : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nome)
                   .IsRequired()
                   .HasColumnType("varchar(200)");

            builder.Property(p => p.Descricao)
                   .IsRequired()
                   .HasColumnType("varchar(1000)");

            builder.Property(p => p.Imagem)
                   .IsRequired()
                   .HasColumnType("varchar(100)");

            builder.Property(p => p.DataCadastro)
                  .IsRequired()
                  .HasColumnType("DateTime");

            builder.Property(p => p.DataAlteracao)
                   .IsRequired()
                   .HasColumnType("DateTime");

            builder.ToTable("Produtos");
        }
    }

}