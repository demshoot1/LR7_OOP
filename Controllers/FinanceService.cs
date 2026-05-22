using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace Controllers
{
    public class FinanceService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ILimitRepository _limitRepository;

        // Внедряем интерфейсы через конструктор (Dependency Injection)
        // Сам C# пока не знает, откуда возьмутся эти репозитории, и это прекрасно!
        public FinanceService(
            IAccountRepository accountRepository,
            ITransactionRepository transactionRepository,
            ILimitRepository limitRepository)
        {
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
            _limitRepository = limitRepository;
        }

        // 1. Создание нового счета
        public async Task<Account> CreateAccountAsync(string name, decimal initialBalance)
        {
            var account = new Account(name, initialBalance);
            await _accountRepository.AddAsync(account);
            return account;
        }

        // 2. Получение всех счетов
        public async Task<IEnumerable<Account>> GetAllAccountsAsync()
        {
            return await _accountRepository.GetAllAsync();
        }

        // 3. Фиксация доходов и расходов (с валидацией лимитов)
        public async Task<(Transaction Transaction, string? Warning)> CreateTransactionAsync(
    Guid accountId,
    TransactionType type,
    decimal amount,
    string category)
        {
            // 1. Проверяем существование счета
            var account = await _accountRepository.GetByIdAsync(accountId);
            if (account == null)
            {
                throw new Exception("Счет не найден.");
            }

            string? warning = null;

            // 2. Логика для РАСХОДА
            if (type == TransactionType.Expense)
            {
                // --- ВОТ ЭТОГО БЛОКА У ВАС НЕ ХВАТАЛО ---
                if (account.Balance < amount)
                {
                    throw new Exception("Недостаточно средств на счете.");
                }
                // ----------------------------------------

                account.Balance -= amount;

                // Логика проверки лимитов...
                var now = DateTime.UtcNow;
                var limit = await _limitRepository.GetByCategoryAndPeriodAsync(category, now.Month, now.Year);
                if (limit != null)
                {
                    limit.AmountSpent += amount;
                    if (limit.AmountSpent > limit.AmountLimit)
                    {
                        warning = $"Внимание! Превышен лимит по категории '{category}'.";
                    }
                    await _limitRepository.UpdateAsync(limit);
                }
            }
            // 3. Логика для ДОХОДА
            else if (type == TransactionType.Income)
            {
                account.Balance += amount;
            }

            // Сохраняем транзакцию и обновляем счет
            var transaction = new Transaction(accountId, type, amount, category);
            await _transactionRepository.AddAsync(transaction);
            await _accountRepository.UpdateAsync(account);

            return (transaction, warning);
        }

        // 4. Установка ежемесячного бюджета (Лимита)
        public async Task<Limit> SetLimitAsync(string category, decimal amountLimit)
        {
            var now = DateTime.UtcNow;
            var existingLimit = await _limitRepository.GetByCategoryAndPeriodAsync(category, now.Month, now.Year);

            if (existingLimit != null)
            {
                existingLimit.AmountLimit = amountLimit;
                await _limitRepository.UpdateAsync(existingLimit);
                return existingLimit;
            }

            var newLimit = new Limit(category, amountLimit, now.Month, now.Year);
            await _limitRepository.AddAsync(newLimit);
            return newLimit;
        }

        // 5. Получение всех текущих лимитов
        public async Task<IEnumerable<Limit>> GetAllLimitsAsync()
        {
            return await _limitRepository.GetAllAsync();
        }

        // 6. Аналитика: Получение сводки расходов за текущий месяц в разрезе категорий
        public async Task<Dictionary<string, decimal>> GetMonthlyAnalyticsAsync()
        {
            var now = DateTime.UtcNow;
            var limits = await _limitRepository.GetAllAsync();

            var analytics = new Dictionary<string, decimal>();

            foreach (var limit in limits)
            {
                if (limit.Month == now.Month && limit.Year == now.Year)
                {
                    analytics[limit.Category] = limit.AmountSpent;
                }
            }

            return analytics;
        }
    }
}