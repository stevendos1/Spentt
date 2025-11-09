// Spendnt.Shared/DTOs/Ingreso/IngresoDto.cs
using System;

namespace Spendnt.Shared.DTOs.Ingreso
{
    public class IngresoDto
    {
        public int Id { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public int CategoriaId { get; set; }
        public string CategoriaNombre { get; set; } = string.Empty;
    }
}
