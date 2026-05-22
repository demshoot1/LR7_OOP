namespace Models
{
    public enum TransactionType
    {
        Income,
        Expense 
    }

    public class Transaction
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; } // К какому счету относится
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public string Category { get; set; } = string.Empty; // Еда, транспорт, досуг...
        public DateTime DateTime { get; set; }

        public Transaction() { }

        public Transaction(Guid accountId, TransactionType type, decimal amount, string category)
        {
            Id = Guid.NewGuid();
            AccountId = accountId;
            Type = type;
            Amount = amount;
            Category = category;
            DateTime = DateTime.UtcNow;
        }
    }
}