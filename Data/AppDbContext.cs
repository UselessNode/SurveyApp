// Data/AppDbContext.cs
using Microsoft.EntityFrameworkCore;
using SurveyApp.Models;

namespace SurveyApp.Data;

// Точка входа в базу данных для EF Core
// (аналог связки JPA/Hibernate в Spring Boot)
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Каждое свойство DbSet — это таблица в базе данных
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<PersonalInfo> PersonalInfos => Set<PersonalInfo>();
    public DbSet<Role> Roles => Set<Role>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // В PostgreSQL принято называть таблицы в snake_case, поэтому задаём имена вручную
        modelBuilder.Entity<Account>().ToTable("accounts");
        modelBuilder.Entity<PersonalInfo>().ToTable("personal_info");
        modelBuilder.Entity<Role>().ToTable("roles");

        // Один к одному: у аккаунта один блок персональных данных.
        // Внешний ключ лежит в таблице personal_info, при удалении аккаунта данные удаляются вместе с ним
        modelBuilder.Entity<Account>()
            .HasOne(a => a.PersonalInfo)
            .WithOne(p => p.Account)
            .HasForeignKey<PersonalInfo>(p => p.AccountId)
            .OnDelete(DeleteBehavior.Cascade);

        // Многие к одному: у аккаунта одна роль, у роли много аккаунтов.
        // Restrict запрещает удалить роль, пока на неё ссылаются аккаунты
        modelBuilder.Entity<Account>()
            .HasOne(a => a.Role)
            .WithMany(r => r.Accounts)
            .HasForeignKey(a => a.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        // Логин уникален — двух пользователей с одинаковым логином быть не должно
        modelBuilder.Entity<Account>()
            .HasIndex(a => a.Login)
            .IsUnique();

        // Роль тоже уникальна по названию, чтобы не появилось двух "USER"
        modelBuilder.Entity<Role>()
            .HasIndex(r => r.Name)
            .IsUnique();
    }
}
