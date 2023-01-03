using System;
using System.Collections.Generic;
using BankTransfer.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BankTransfer.Infrastructure.Context
{
    public partial class CoreBankingContext : DbContext
    {
        public CoreBankingContext()
        {
        }

        public CoreBankingContext(DbContextOptions<CoreBankingContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Transaction> Transactions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("Transaction");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.BeneficiaryAccountNumber)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.BeneficiaryBankCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CallBackUrl)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.CurrencyCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Guid).IsUnicode(false);

                entity.Property(e => e.Provider)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ResponseCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ResponseMessage)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.TransactionDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("transactionDateTime");

                entity.Property(e => e.TransactionReference)
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
