// models/SurveyResponse.cs
namespace SurveyApp.Models;

public class SurveyResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int Age { get; set; }
    public string Comments { get; set; } = "";
    public DateTime SubmittedAt { get; set; } = DateTime.Now;

    public List<ResponseOption> ResponseOptions { get; set; } = new();
}
