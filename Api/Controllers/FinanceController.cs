using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Controllers;
using Models;
using Shared; // Подключаем наши DTO

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FinanceController : ControllerBase
    {
        private readonly FinanceService _financeService;

        public FinanceController(FinanceService financeService)
        {
            _financeService = financeService;
        }

        // POST: api/finance/accounts
        [HttpPost("accounts")]
        public async Task<ActionResult<AccountResponseDto>> CreateAccount([FromForm] CreateAccountRequest request)
        {
            var account = await _financeService.CreateAccountAsync(request.Name, request.InitialBalance);

            var response = new AccountResponseDto
            {
                Id = account.Id,
                Name = account.Name,
                Balance = account.Balance
            };

            return Ok(response);
        }

        // GET: api/finance/accounts
        [HttpGet("accounts")]
        public async Task<ActionResult<IEnumerable<AccountResponseDto>>> GetAccounts()
        {
            var accounts = await _financeService.GetAllAccountsAsync();

            var response = accounts.Select(a => new AccountResponseDto
            {
                Id = a.Id,
                Name = a.Name,
                Balance = a.Balance
            });

            return Ok(response);
        }

        // POST: api/finance/transactions
        [HttpPost("transactions")]
        public async Task<ActionResult<TransactionResultDto>> CreateTransaction([FromForm] CreateTransactionRequest request)
        {
            try
            {
                // Парсим строку в наш Enum предметной области
                if (!Enum.TryParse<TransactionType>(request.Type, true, out var transactionType))
                {
                    return BadRequest("Некорректный тип транзакции. Используйте 'Income' или 'Expense'");
                }

                var result = await _financeService.CreateTransactionAsync(
                    request.AccountId,
                    transactionType,
                    request.Amount,
                    request.Category
                );

                var transactionDto = new TransactionResponseDto
                {
                    Id = result.Transaction.Id,
                    AccountId = result.Transaction.AccountId,
                    Type = result.Transaction.Type.ToString(),
                    Amount = result.Transaction.Amount,
                    Category = result.Transaction.Category,
                    DateTime = result.Transaction.DateTime
                };

                var response = new TransactionResultDto
                {
                    Transaction = transactionDto,
                    Warning = result.Warning
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/finance/limits
        [HttpPost("limits")]
        public async Task<ActionResult<LimitResponseDto>> SetLimit([FromForm] SetLimitRequest request)
        {
            var limit = await _financeService.SetLimitAsync(request.Category, request.AmountLimit);

            var response = new LimitResponseDto
            {
                Category = limit.Category,
                AmountLimit = limit.AmountLimit,
                AmountSpent = limit.AmountSpent,
                IsExceeded = limit.IsExceeded
            };

            return Ok(response);
        }

        // GET: api/finance/limits
        [HttpGet("limits")]
        public async Task<ActionResult<IEnumerable<LimitResponseDto>>> GetLimits()
        {
            var limits = await _financeService.GetAllLimitsAsync();

            var response = limits.Select(l => new LimitResponseDto
            {
                Category = l.Category,
                AmountLimit = l.AmountLimit,
                AmountSpent = l.AmountSpent,
                IsExceeded = l.IsExceeded
            });

            return Ok(response);
        }

        // GET: api/finance/analytics
        [HttpGet("analytics")]
        public async Task<IActionResult> GetAnalytics()
        {
            var analytics = await _financeService.GetMonthlyAnalyticsAsync();
            return Ok(analytics);
        }
    }
}