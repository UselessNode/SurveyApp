// Repositories/RoleRepository.cs
using Microsoft.EntityFrameworkCore;
using SurveyApp.Data;
using SurveyApp.Models;

namespace SurveyApp.Repositories;

// Реализация репозитория ролей
public class RoleRepository : IRoleRepository
{
    private readonly AppDbContext _db;

    public RoleRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Role>> GetAllAsync()
    {
        return await _db.Roles.OrderBy(r => r.Id).ToListAsync();
    }

    public async Task<Role?> GetByNameAsync(string name)
    {
        return await _db.Roles.FirstOrDefaultAsync(r => r.Name == name);
    }

    public async Task AddAsync(Role role)
    {
        _db.Roles.Add(role);
        await _db.SaveChangesAsync();
    }
}
