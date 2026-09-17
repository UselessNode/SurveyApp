// Dtos/CreateAccountRequest.cs
using System.ComponentModel.DataAnnotations;
using SurveyApp.Models;

namespace SurveyApp.Dtos;

// Данные, которые приходят в POST /api/accounts.
// Аналог @RequestBody в Spring: фреймворк сам разбирает JSON в этот объект
public class CreateAccountRequest
{
    [Required(ErrorMessage = "Логин обязателен")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Логин должен быть от 3 до 50 символов")]
    public string Login { get; set; } = "";

    [Required(ErrorMessage = "Пароль обязателен")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль должен быть от 6 до 100 символов")]
    public string Password { get; set; } = "";

    [Required(ErrorMessage = "Блок personalInfo обязателен")]
    public PersonalInfoRequest? PersonalInfo { get; set; }

    [Required(ErrorMessage = "Блок role обязателен")]
    public RoleRequest? Role { get; set; }

    // Превращаем DTO в сущность, с которой работает слой сервисов
    public Account ToAccount()
    {
        return new Account
        {
            Login = Login.Trim(),
            Password = Password,
            PersonalInfo = new PersonalInfo
            {
                FullName = PersonalInfo!.FullName.Trim(),
                Email = PersonalInfo.Email.Trim()
            },
            // Роль передаётся только по имени: id подставит сервис, найдя роль в справочнике
            Role = new Role { Name = Role!.Name.Trim() }
        };
    }
}

// Вложенный объект "personalInfo"
public class PersonalInfoRequest
{
    [Required(ErrorMessage = "ФИО обязательно")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "ФИО должно быть от 2 до 150 символов")]
    public string FullName { get; set; } = "";

    [Required(ErrorMessage = "Email обязателен")]
    [EmailAddress(ErrorMessage = "Некорректный email")]
    [StringLength(150, ErrorMessage = "Email не длиннее 150 символов")]
    public string Email { get; set; } = "";
}

// Вложенный объект "role"
public class RoleRequest
{
    [Required(ErrorMessage = "Название роли обязательно")]
    [StringLength(30, MinimumLength = 2, ErrorMessage = "Название роли должно быть от 2 до 30 символов")]
    public string Name { get; set; } = "";
}
