namespace Models
{
    public class Limit
    {
        public Guid Id { get; set; }
        public string Category { get; set; } = string.Empty;
        public decimal AmountLimit { get; set; } // Максимальный бюджет
        public decimal AmountSpent { get; set; } // Сколько уже потрачено
        public int Month { get; set; }
        public int Year { get; set; }

        public Limit() { }

        public Limit(string category, decimal amountLimit, int month, int year)
        {
            Id = Guid.NewGuid();
            Category = category;
            AmountLimit = amountLimit;
            AmountSpent = 0;
            Month = month;
            Year = year;
        }

        // Вспомогательный метод для проверки превышения
        public bool IsExceeded => AmountSpent > AmountLimit;
    }
}