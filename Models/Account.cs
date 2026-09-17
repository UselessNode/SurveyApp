// Models/Account.cs
namespace SurveyApp.Models;

// Основная учётная запись (таблица accounts)
public class Account
{
    public int Id { get; set; }

    // Логин, по которому пользователь входит в систему
    public string Login { get; set; } = "";

    // Пароль. В учебном проекте хранится как есть, в реальном — только хеш
    public string Password { get; set; } = "";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Связь "один к одному": у аккаунта ровно один блок персональных данных
    public PersonalInfo? PersonalInfo { get; set; }

    // Связь "многие к одному": у аккаунта одна роль, а у роли может быть много аккаунтов
    public int RoleId { get; set; }
    public Role? Role { get; set; }
}
