﻿using PasswordManager.PaymentCard.Infrastructure.OperationRepository;
using Microsoft.EntityFrameworkCore;

namespace PasswordManager.PaymentCard.Infrastructure.PaymentCardRepository;
public class PaymentCardContext : DbContext
{
    public PaymentCardContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<OperationEntity>? Operations { get; set; }
    public DbSet<PaymentCardEntity>? PaymentCards { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new OperationEntityConfiguration());
        modelBuilder.ApplyConfiguration(new PaymentCardConfiguration());
    }
}