// Pages/Index.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SurveyApp.Data;
using SurveyApp.Models;

namespace SurveyApp.Pages;

public class IndexModel : PageModel
{
    private readonly AppDbContext _db;

    [BindProperty]
    public string Name { get; set; } = "";

    [BindProperty]
    public int Age { get; set; }

    [BindProperty]
    public string Comments { get; set; } = "";

    // Список ID выбранных вариантов (приходит из формы)
    [BindProperty]
    public List<int> SelectedOptionIds { get; set; } = new();

    public List<VoteOption> AllOptions { get; set; } = new();
    public string? Message { get; set; }

    public IndexModel(AppDbContext db)
    {
        _db = db;
    }

    public async Task OnGetAsync()
    {
        AllOptions = await _db.VoteOptions.ToListAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid || !SelectedOptionIds.Any())
        {
            AllOptions = await _db.VoteOptions.ToListAsync();
            Message = "Выберите хотя бы один вариант!";
            return Page();
        }

        var response = new SurveyResponse
        {
            Name = Name,
            Age = Age,
            Comments = Comments,
            SubmittedAt = DateTime.Now
        };

        // Создаем связи
        foreach (var optionId in SelectedOptionIds)
        {
            response.ResponseOptions.Add(new ResponseOption
            {
                VoteOptionId = optionId
            });
        }

        _db.SurveyResponses.Add(response);
        await _db.SaveChangesAsync();

        Message = "Спасибо! Ответ сохранен.";

        // Очистка полей
        Name = "";
        Age = 0;
        Comments = "";
        SelectedOptionIds.Clear();

        AllOptions = await _db.VoteOptions.ToListAsync();
        return Page();
    }
}
