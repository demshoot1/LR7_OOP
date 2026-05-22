using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Models
{
    public interface ITransactionRepository
    {
        Task<Transaction?> GetByIdAsync(Guid id);
        Task<IEnumerable<Transaction>> GetByAccountIdAsync(Guid accountId);
        Task<IEnumerable<Transaction>> GetByCategoryAsync(string category, int month, int year);
        Task AddAsync(Transaction transaction);
    }
}