// Exceptions/ApiExceptionHandler.cs
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace SurveyApp.Exceptions;

// Единый обработчик ошибок: сервисы просто кидают исключения,
// а этот класс решает, какой HTTP-код вернуть клиенту.
// В Spring Boot похожую роль играет @ControllerAdvice
public class ApiExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        // Определяем код ответа по типу исключения
        var (statusCode, title) = exception switch
        {
            NotFoundException => (StatusCodes.Status404NotFound, "Запись не найдена"),
            ConflictException => (StatusCodes.Status409Conflict, "Конфликт данных"),
            _ => (0, "")
        };

        // Неизвестные ошибки не трогаем — пусть обрабатываются стандартно (500)
        if (statusCode == 0)
        {
            return false;
        }

        httpContext.Response.StatusCode = statusCode;

        // ProblemDetails — стандартный формат описания ошибки в ASP.NET Core
        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = exception.Message,
            Instance = httpContext.Request.Path
        }, cancellationToken);

        return true;
    }
}
