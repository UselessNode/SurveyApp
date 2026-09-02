// Pages/Stats.cshtml.cs
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SurveyApp.Models;
using SurveyApp.Data;

namespace SurveyApp.Pages;

public class StatsModel : PageModel
{
    private readonly AppDbContext _db;

    public int TotalResponses { get; set; }
    public List<OptionStat> Stats { get; set; } = new();

    public StatsModel(AppDbContext db)
    {
        _db = db;
    }

    public async Task OnGetAsync()
    {
        TotalResponses = await _db.SurveyResponses.CountAsync();

        // Группировка по вариантам ответа
        Stats = await _db.ResponseOptions
            .Include(ro => ro.VoteOption)
            .GroupBy(ro => ro.VoteOption!.OptionText)
            .Select(g => new OptionStat
            {
                OptionName = g.Key!,
                Count = g.Count()
            })
            .OrderByDescending(s => s.Count)
            .ToListAsync();
    }
}

public class OptionStat
{
    public string OptionName { get; set; } = "";
    public int Count { get; set; }
}
