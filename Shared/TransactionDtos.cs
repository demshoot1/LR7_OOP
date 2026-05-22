using System;
using System.ComponentModel.DataAnnotations;

namespace Shared
{
    public class CreateTransactionRequest
    {
        [Required(ErrorMessage = "Идентификатор счета обязателен.")]
        public Guid AccountId { get; set; }

        [Required(ErrorMessage = "Тип транзакции (Income/Expense) обязателен.")]
        public string Type { get; set; } = string.Empty;

        [Range(0.01, 1000000000, ErrorMessage = "Сумма транзакции должна быть больше нуля.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Категория транзакции обязательна.")]
        [StringLength(30, ErrorMessage = "Название категории слишком длинное (макс. 30 символов).")]
        public string Category { get; set; } = string.Empty;
    }

    public class TransactionResponseDto
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public string Type { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Category { get; set; } = string.Empty;
        public DateTime DateTime { get; set; }
    }

    public class TransactionResultDto
    {
        public TransactionResponseDto Transaction { get; set; } = null!;
        public string? Warning { get; set; }
    }
}