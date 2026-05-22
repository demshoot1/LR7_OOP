using System;
using System.ComponentModel.DataAnnotations;

namespace Shared
{
    public class CreateAccountRequest
    {
        [Required(ErrorMessage = "Название счета обязательно для заполнения.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Название счета должно быть от 2 до 50 символов.")]
        public string Name { get; set; } = string.Empty;

        [Range(0, 1000000000, ErrorMessage = "Начальный баланс не может быть отрицательным.")]
        public decimal InitialBalance { get; set; }
    }

    public class AccountResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Balance { get; set; }
    }
}