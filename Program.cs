// Program.cs
using Microsoft.EntityFrameworkCore;
using SurveyApp.Data;
using SurveyApp.Models;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Seed
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    if (!db.VoteOptions.Any())
    {
        db.VoteOptions.AddRange(
            new VoteOption { OptionText = "Spring Boot (Java)" },
            new VoteOption { OptionText = "FastAPI (Python)" },
            new VoteOption { OptionText = "Django (Python)" },
            new VoteOption { OptionText = "NestJS (TypeScript)" },
            new VoteOption { OptionText = "Express.js (JavaScript)" },
            new VoteOption { OptionText = "Laravel (PHP)" },
            new VoteOption { OptionText = "ASP.NET Core (C#)" },
            new VoteOption { OptionText = "Ruby on Rails (Ruby)" },
            new VoteOption { OptionText = "Gin (Go)" }
        );
        db.SaveChanges();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets();

app.Run();
