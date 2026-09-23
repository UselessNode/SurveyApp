// Repositories/IRoleRepository.cs
using SurveyApp.Models;

namespace SurveyApp.Repositories;

// Контракт репозитория ролей
public interface IRoleRepository
{
    // Все роли справочника
    Task<List<Role>> GetAllAsync();

    // Кастомный запрос: найти роль по названию
    Task<Role?> GetByNameAsync(string name);

    // Добавить новую роль в справочник
    Task AddAsync(Role role);
}
