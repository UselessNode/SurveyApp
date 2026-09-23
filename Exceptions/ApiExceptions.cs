// Exceptions/ApiExceptions.cs
namespace SurveyApp.Exceptions;

// Исключение "запись не найдена" — контроллер вернёт 404.
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}

// Исключение конфликта данных (например, логин уже занят) — контроллер вернёт 409
public class ConflictException : Exception
{
    public ConflictException(string message) : base(message) { }
}
