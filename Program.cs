// Program.cs
using Microsoft.EntityFrameworkCore;
using SurveyApp.Data;
using SurveyApp.Exceptions;
using SurveyApp.Models;
using SurveyApp.Repositories;
using SurveyApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Подключаем контроллеры REST API и единый обработчик ошибок
builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ApiExceptionHandler>();

// Подключение к PostgreSQL.
// Аналог настроек spring.datasource.* в файле application.properties.
// Сама строка подключения лежит в appsettings.json / appsettings.Development.json
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Регистрация слоёв приложения: репозитории (работа с БД) и сервисы (бизнес-логика).
// AddScoped — на каждый HTTP-запрос создаётся свой экземпляр, как @Repository и @Service в Spring
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();

var app = builder.Build();

// Аналог spring.jpa.hibernate.ddl-auto=update:
// при старте приложения доводим структуру таблиц в БД до текущей модели
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    // Справочник ролей заполняем один раз
    if (!db.Roles.Any())
    {
        db.Roles.AddRange(
            new Role { Name = "ADMIN" },
            new Role { Name = "USER" }
        );
        db.SaveChanges();
    }
}

app.UseExceptionHandler();
app.MapControllers();

app.Run();
