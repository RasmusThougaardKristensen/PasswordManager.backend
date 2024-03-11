﻿using PasswordManager.Password.Infrastructure.OperationRepository;

namespace PasswordManager.Password.Infrastructure.PasswordRepository;
public class PasswordContext : DbContext
{
    public PasswordContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<OperationEntity>? Operations { get; set; }
    public DbSet<PasswordEntity>? Passwords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new OperationEntityConfiguration());
        modelBuilder.ApplyConfiguration(new PasswordConfiguration());
    }
}
