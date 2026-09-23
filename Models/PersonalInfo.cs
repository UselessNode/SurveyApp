// Models/PersonalInfo.cs
namespace SurveyApp.Models;

// Персональные данные пользователя (таблица personal_info)
public class PersonalInfo
{
    public int Id { get; set; }

    // ФИО пользователя. Все кто делают отдельные поля под Фамилию, Имя, Отчество -- кретины
    public string FullName { get; set; } = "";

    public string Email { get; set; } = "";

    // Внешний ключ. Связь "один к одному" хранит ссылку на accounts
    public int AccountId { get; set; }
    public Account? Account { get; set; }
}
