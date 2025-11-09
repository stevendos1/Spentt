// Spendnt.Shared/DTOs/RecordatorioGasto/RecordatorioGastoCreateDto.cs
using System;
using System.ComponentModel.DataAnnotations;

#nullable enable

namespace Spendnt.Shared.DTOs.RecordatorioGasto
{
    public class RecordatorioGastoCreateDto
    {
        [Required]
        [MaxLength(200)]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto estimado debe ser mayor a cero.")]
        public decimal MontoEstimado { get; set; }

        [Required]
        public DateTime FechaProgramada { get; set; }

        [MaxLength(500)]
        public string? Notas { get; set; }
    }
}
