// Spendnt.Shared/DTOs/Historial/HistorialUpdateDto.cs
using System;
using System.ComponentModel.DataAnnotations;

#nullable enable

namespace Spendnt.Shared.DTOs.Historial
{
    public class HistorialUpdateDto
    {
        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a cero.")]
        public decimal Monto { get; set; }

        [Required]
        [MaxLength(50)]
        public string Tipo { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Descripcion { get; set; }

        [Required]
        public int CategoriaId { get; set; }
    }
}
