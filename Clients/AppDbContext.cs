using Microsoft.EntityFrameworkCore;
using Models;

namespace Clients
{
    public class AppDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; } = null!;
        public DbSet<Transaction> Transactions { get; set; } = null!;
        public DbSet<Limit> Limits { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            // Автоматически создает файл базы данных и накатывает таблицы при старте, 
            // если файла еще нет. Очень удобно для лаб!
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Здесь можно явно указать правила, если нужно.
            // Например, точность для финансовых типов (decimal)
            modelBuilder.Entity<Account>().Property(a => a.Balance).HasConversion<double>();
            modelBuilder.Entity<Transaction>().Property(t => t.Amount).HasConversion<double>();
            modelBuilder.Entity<Limit>().Property(l => l.AmountLimit).HasConversion<double>();
            modelBuilder.Entity<Limit>().Property(l => l.AmountSpent).HasConversion<double>();
        }
    }
}