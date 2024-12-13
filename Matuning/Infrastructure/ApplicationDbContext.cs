using Matuning.Domain.Models.Cars;
using Matuning.Domain.Models.Reports;
using Matuning.Domain.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace Matuning.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public DbSet<Report> Reports { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Car> Cars { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Дополнительная конфигурация модели при необходимости
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Пример настройки сущности User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId);
            entity.Property(e => e.Username).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
            entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(256);
            entity.Property(e => e.RegistrationDate).IsRequired();
            // Добавьте другие настройки по необходимости
        });

        // Пример настройки сущности Report
        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.ReportId);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).IsRequired();
            entity.Property(e => e.Timestamp).IsRequired();
            // Добавьте другие настройки по необходимости
        });

        // Добавьте другие сущности и их настройки по мере необходимости
    }
}