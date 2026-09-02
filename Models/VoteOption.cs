// models/VoteOption.cs
namespace SurveyApp.Models;

public class VoteOption
{
    public int Id { get; set; }
    public string OptionText { get; set; } = "";

    public List<SurveyResponse> SurveyResponses { get; set; } = new();
}
