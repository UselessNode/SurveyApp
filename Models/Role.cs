// Models/Role.cs
namespace SurveyApp.Models;

// Роль пользователя (таблица roles) — это справочник: ADMIN, USER и т.д.
public class Role
{
    public int Id { get; set; }

    // Название роли, например "USER"
    public string Name { get; set; } = "";

    // Обратная сторона связи "многие к одному": у одной роли много аккаунтов
    public List<Account> Accounts { get; set; } = new();
}
