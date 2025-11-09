// Spendnt.Shared/DTOs/Egreso/EgresoDto.cs
using System;

namespace Spendnt.Shared.DTOs.Egreso
{
    public class EgresoDto
    {
        public int Id { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public int CategoriaId { get; set; }
        public string CategoriaNombre { get; set; } = string.Empty;
    }
}
