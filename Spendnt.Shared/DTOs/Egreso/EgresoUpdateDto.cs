// Spendnt.Shared/DTOs/Egreso/EgresoUpdateDto.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace Spendnt.Shared.DTOs.Egreso
{
    public class EgresoUpdateDto
    {
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El egreso debe ser mayor a cero.")]
        public decimal Monto { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public int CategoriaId { get; set; }
    }
}
