// Spendnt.Shared/DTOs/Ingreso/IngresoUpdateDto.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace Spendnt.Shared.DTOs.Ingreso
{
    public class IngresoUpdateDto
    {
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El ingreso debe ser mayor a cero.")]
        public decimal Monto { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public int CategoriaId { get; set; }
    }
}
