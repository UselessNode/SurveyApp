// Models/Account.cs
namespace SurveyApp.Models;

// Основная учётная запись (таблица accounts)
public class Account
{
    public int Id { get; set; }

    public string Login { get; set; } = "";

    // TODO: Избавиться от уязвимости -> Храним хэш вместо строки пароля.
    public string Password { get; set; } = "";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Связь "один к одному": у аккаунта ровно один блок персональных данных
    public PersonalInfo? PersonalInfo { get; set; }

    // Связь "многие к одному": у аккаунта одна роль, а у роли может быть много аккаунтов
    public int RoleId { get; set; }
    public Role? Role { get; set; }
}
