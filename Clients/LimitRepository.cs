using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Models;

namespace Clients
{
    public class LimitRepository : ILimitRepository
    {
        private readonly AppDbContext _context;

        public LimitRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Limit?> GetByCategoryAndPeriodAsync(string category, int month, int year)
        {
            return await _context.Limits
                .FirstOrDefaultAsync(l => l.Category.ToLower() == category.ToLower() &&
                                          l.Month == month &&
                                          l.Year == year);
        }

        public async Task<IEnumerable<Limit>> GetAllAsync()
        {
            return await _context.Limits.ToListAsync();
        }

        public async Task AddAsync(Limit limit)
        {
            await _context.Limits.AddAsync(limit);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Limit limit)
        {
            _context.Limits.Update(limit);
            await _context.SaveChangesAsync();
        }
    }
}