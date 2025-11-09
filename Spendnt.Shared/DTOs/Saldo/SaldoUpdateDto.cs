// Spendnt.Shared/DTOs/Saldo/SaldoUpdateDto.cs
using System.ComponentModel.DataAnnotations;

#nullable enable

namespace Spendnt.Shared.DTOs.Saldo
{
    public class SaldoUpdateDto
    {
        [Required]
        public decimal TotalSaldo { get; set; }
    }
}
