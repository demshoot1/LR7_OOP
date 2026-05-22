using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Models
{
    public interface ILimitRepository
    {
        Task<Limit?> GetByCategoryAndPeriodAsync(string category, int month, int year);
        Task<IEnumerable<Limit>> GetAllAsync();
        Task AddAsync(Limit limit);
        Task UpdateAsync(Limit limit);
    }
}