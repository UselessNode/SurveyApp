// Repositories/IAccountRepository.cs
using SurveyApp.Models;

namespace SurveyApp.Repositories;

// Контракт репозитория аккаунтов.
// Полный аналог интерфейса, наследующего JpaRepository<Account, Integer> в Spring Data JPA
public interface IAccountRepository
{
    // Получить все записи
    Task<List<Account>> GetAllAsync();

    // Получить одну запись по первичному ключу
    Task<Account?> GetByIdAsync(int id);

    // Кастомный запрос: поиск по логину
    Task<Account?> GetByLoginAsync(string login);

    // Кастомный запрос: поиск по ФИО (без учёта регистра, частичное совпадение)
    Task<List<Account>> GetByFullNameAsync(string fullName);

    // Кастомный запрос: получить всех пользователей с указанной ролью
    Task<List<Account>> GetByRoleNameAsync(string roleName);

    // Проверка, занят ли логин (нужна перед созданием пользователя)
    Task<bool> ExistsByLoginAsync(string login);

    // Сохранить нового пользователя (INSERT)
    Task AddAsync(Account account);
}
