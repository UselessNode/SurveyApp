// Repositories/AccountRepository.cs
using Microsoft.EntityFrameworkCore;
using SurveyApp.Data;
using SurveyApp.Models;

namespace SurveyApp.Repositories;

// Реализация репозитория аккаунтов на EF Core.
// В Spring Data JPA этот класс писать не нужно — интерфейса достаточно.
// В .NET мы сами пишем запросы, поэтому репозиторий делается вручную
public class AccountRepository : IAccountRepository
{
    private readonly AppDbContext _db;

    public AccountRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Account>> GetAllAsync()
    {
        // Include подгружает связанные таблицы (персональные данные и роль),
        // иначе в JSON попали бы только id роли и аккаунта
        return await _db.Accounts
            .Include(a => a.PersonalInfo)
            .Include(a => a.Role)
            .OrderBy(a => a.Id)
            .ToListAsync();
    }

    public async Task<Account?> GetByIdAsync(int id)
    {
        return await _db.Accounts
            .Include(a => a.PersonalInfo)
            .Include(a => a.Role)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Account?> GetByLoginAsync(string login)
    {
        return await _db.Accounts
            .Include(a => a.PersonalInfo)
            .Include(a => a.Role)
            .FirstOrDefaultAsync(a => a.Login == login);
    }

    public async Task<List<Account>> GetByFullNameAsync(string fullName)
    {
        // EF.Functions.ILike — регистронезависимый поиск, есть только в PostgreSQL.
        // % вокруг строки означает "содержит подстроку"
        var pattern = $"%{fullName}%";

        return await _db.Accounts
            .Include(a => a.PersonalInfo)
            .Include(a => a.Role)
            .Where(a => a.PersonalInfo != null && EF.Functions.ILike(a.PersonalInfo.FullName, pattern))
            .OrderBy(a => a.Id)
            .ToListAsync();
    }

    public async Task<List<Account>> GetByRoleNameAsync(string roleName)
    {
        return await _db.Accounts
            .Include(a => a.PersonalInfo)
            .Include(a => a.Role)
            .Where(a => a.Role != null && a.Role.Name == roleName)
            .OrderBy(a => a.Id)
            .ToListAsync();
    }

    public async Task<bool> ExistsByLoginAsync(string login)
    {
        return await _db.Accounts.AnyAsync(a => a.Login == login);
    }

    public async Task AddAsync(Account account)
    {
        _db.Accounts.Add(account);
        await _db.SaveChangesAsync(); // здесь Hibernate/EF сам генерирует INSERT
    }
}
