// Spendnt.Shared/DTOs/TransaccionAhorro/TransaccionAhorroCreateDto.cs
using System.ComponentModel.DataAnnotations;

#nullable enable

namespace Spendnt.Shared.DTOs.TransaccionAhorro
{
    public class TransaccionAhorroCreateDto
    {
        [Required]
        public decimal Monto { get; set; }

        [MaxLength(200)]
        public string? Descripcion { get; set; }
    }
}
