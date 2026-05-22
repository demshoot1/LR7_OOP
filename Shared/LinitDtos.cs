using System.ComponentModel.DataAnnotations;

namespace Shared
{
    public class SetLimitRequest
    {
        [Required(ErrorMessage = "Категория для лимита обязательна.")]
        public string Category { get; set; } = string.Empty;

        [Range(1, 1000000000, ErrorMessage = "Сумма лимита должна быть не менее 1.")]
        public decimal AmountLimit { get; set; }
    }

    public class LimitResponseDto
    {
        public string Category { get; set; } = string.Empty;
        public decimal AmountLimit { get; set; }
        public decimal AmountSpent { get; set; }
        public bool IsExceeded { get; set; }
    }
}