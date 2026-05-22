namespace Models
{
    public class Account
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Balance { get; set; }

        // Конструктор по умолчанию нужен для различных ORM (например, EF Core)
        public Account() { }

        public Account(string name, decimal initialBalance = 0)
        {
            Id = Guid.NewGuid();
            Name = name;
            Balance = initialBalance;
        }
    }
}