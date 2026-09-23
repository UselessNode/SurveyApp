// Controllers/AccountsController.cs
using Microsoft.AspNetCore.Mvc;
using SurveyApp.Dtos;
using SurveyApp.Services;

namespace SurveyApp.Controllers;

// REST-контроллер для работы с аккаунтами.
[ApiController]
[Route("api/accounts")]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountsController(IAccountService accountService)
    {
        _accountService = accountService;
    }

   
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<AccountResponse>> CreateAccount([FromBody] CreateAccountRequest request)
    {
        var account = await _accountService.CreateAccountAsync(request.ToAccount());

        // 201 Created и ссылка на только что созданную запись
        return CreatedAtAction(
            nameof(GetAccountById),
            new { id = account.Id },
            AccountResponse.FromAccount(account));
    }

    // GET /api/accounts — получение всех пользователей.
    // Необязательные параметры ?fullName=... и ?role=... включают кастомные запросы поиска
    [HttpGet]
    public async Task<ActionResult<List<AccountResponse>>> GetAllAccounts(
        [FromQuery] string? fullName,
        [FromQuery] string? role)
    {
        var accounts = string.IsNullOrWhiteSpace(fullName) && string.IsNullOrWhiteSpace(role)
            ? await _accountService.GetAllAccountsAsync()
            : await _accountService.SearchAccountsAsync(fullName, role);

        return Ok(accounts.Select(AccountResponse.FromAccount).ToList());
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AccountResponse>> GetAccountById(int id)
    {
        var account = await _accountService.GetAccountByIdAsync(id);

        return Ok(AccountResponse.FromAccount(account));
    }
}
