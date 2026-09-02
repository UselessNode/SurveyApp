// Data/AppDbContext.cs
using Microsoft.EntityFrameworkCore;
using SurveyApp.Models;
namespace SurveyApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<SurveyResponse> SurveyResponses => Set<SurveyResponse>();
    public DbSet<VoteOption> VoteOptions => Set<VoteOption>();
    public DbSet<ResponseOption> ResponseOptions => Set<ResponseOption>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Настраиваем уникальность пары (Response + Option), чтобы нельзя было выбрать один вариант дважды
        modelBuilder.Entity<ResponseOption>()
            .HasIndex(ro => new { ro.SurveyResponseId, ro.VoteOptionId })
            .IsUnique();
    }
}
