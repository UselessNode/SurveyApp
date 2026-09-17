// Services/IAccountService.cs
using SurveyApp.Models;

namespace SurveyApp.Services;

// Сервисный слой: бизнес-логика приложения.
// Аналог интерфейса сервиса в Spring Boot (реализацию помечают аннотацией @Service)
public interface IAccountService
{
    // Создать нового пользователя (с проверками: логин уникален, роль существует)
    Task<Account> CreateAccountAsync(Account account);

    // Просто сохранить пользователя в БД
    Task<Account> SaveAccountAsync(Account account);

    // Получить всех пользователей
    Task<List<Account>> GetAllAccountsAsync();

    // Найти пользователей по ФИО или по роли
    Task<List<Account>> SearchAccountsAsync(string? fullName, string? roleName);

    // Получить одного пользователя по ID
    Task<Account> GetAccountByIdAsync(int id);
}
