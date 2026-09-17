// Services/AccountService.cs
using SurveyApp.Exceptions;
using SurveyApp.Models;
using SurveyApp.Repositories;

namespace SurveyApp.Services;

// Реализация сервиса аккаунтов.
// Репозитории внедряются через конструктор — так же, как это делается в Spring Boot
public class AccountService : IAccountService
{
    private readonly IAccountRepository _accounts;
    private readonly IRoleRepository _roles;

    public AccountService(IAccountRepository accounts, IRoleRepository roles)
    {
        _accounts = accounts;
        _roles = roles;
    }

    public async Task<Account> CreateAccountAsync(Account account)
    {
        // Логин должен быть уникальным: если такой уже есть — это конфликт (409)
        if (await _accounts.ExistsByLoginAsync(account.Login))
        {
            throw new ConflictException($"Пользователь с логином '{account.Login}' уже существует");
        }

        // В JSON приходит только название роли, поэтому находим её в справочнике
        await AttachRoleAsync(account);

        return await SaveAccountAsync(account);
    }

    public async Task<Account> SaveAccountAsync(Account account)
    {
        // Запоминаем время создания записи
        account.CreatedAt = DateTime.UtcNow;

        // Сохраняем нового пользователя в БД (INSERT в таблицу accounts)
        await _accounts.AddAsync(account);
        return account;
    }

    public async Task<List<Account>> GetAllAccountsAsync()
    {
        return await _accounts.GetAllAsync();
    }

    public async Task<List<Account>> SearchAccountsAsync(string? fullName, string? roleName)
    {
        // Используем кастомные запросы репозитория
        if (!string.IsNullOrWhiteSpace(roleName))
        {
            return await _accounts.GetByRoleNameAsync(roleName.Trim());
        }

        if (!string.IsNullOrWhiteSpace(fullName))
        {
            return await _accounts.GetByFullNameAsync(fullName.Trim());
        }

        return await _accounts.GetAllAsync();
    }

    public async Task<Account> GetAccountByIdAsync(int id)
    {
        // Ищем пользователя по ID, если нет — кидаем исключение (контроллер вернёт 404)
        var account = await _accounts.GetByIdAsync(id);

        if (account is null)
        {
            throw new NotFoundException($"Пользователь с id={id} не найден");
        }

        return account;
    }

    // Подставляет аккаунту реальную роль из справочника.
    // Если роли с таким названием ещё нет — она будет создана вместе с аккаунтом
    private async Task AttachRoleAsync(Account account)
    {
        if (account.Role is null || string.IsNullOrWhiteSpace(account.Role.Name))
        {
            throw new ConflictException("Не указана роль пользователя");
        }

        var roleName = account.Role.Name;
        var existingRole = await _roles.GetByNameAsync(roleName);

        // Ссылаемся на существующую роль, иначе создаём новую — так в справочнике не будет дублей
        account.Role = existingRole ?? new Role { Name = roleName };
    }
}
