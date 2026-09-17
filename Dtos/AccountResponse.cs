// Dtos/AccountResponse.cs
using SurveyApp.Models;

namespace SurveyApp.Dtos;

// То, что уходит клиенту в ответ на GET/POST.
// Отдаём не сущность, а отдельный объект: так мы не показываем пароль
// и не уходим в бесконечную рекурсию "аккаунт -> роль -> список аккаунтов"
public class AccountResponse
{
    public int Id { get; set; }
    public string Login { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public PersonalInfoResponse? PersonalInfo { get; set; }
    public RoleResponse? Role { get; set; }

    // Собираем ответ из сущности
    public static AccountResponse FromAccount(Account account)
    {
        return new AccountResponse
        {
            Id = account.Id,
            Login = account.Login,
            CreatedAt = account.CreatedAt,
            PersonalInfo = account.PersonalInfo is null
                ? null
                : new PersonalInfoResponse
                {
                    Id = account.PersonalInfo.Id,
                    FullName = account.PersonalInfo.FullName,
                    Email = account.PersonalInfo.Email
                },
            Role = account.Role is null
                ? null
                : new RoleResponse
                {
                    Id = account.Role.Id,
                    Name = account.Role.Name
                }
        };
    }
}

public class PersonalInfoResponse
{
    public int Id { get; set; }
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
}

public class RoleResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}
