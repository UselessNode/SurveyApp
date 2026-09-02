// Models/ResponseOption.cs
using SurveyApp.Models;
namespace SurveyApp.Models;

public class ResponseOption
{
    public int Id { get; set; }

    public int SurveyResponseId { get; set; }
    public SurveyResponse? SurveyResponse { get; set; }

    public int VoteOptionId { get; set; }
    public VoteOption? VoteOption { get; set; }
}
