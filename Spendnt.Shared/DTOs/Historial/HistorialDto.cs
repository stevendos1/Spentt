// Spendnt.Shared/DTOs/Historial/HistorialDto.cs
using System;

#nullable enable

namespace Spendnt.Shared.DTOs.Historial
{
    public class HistorialDto
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Monto { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public int CategoriaId { get; set; }
        public string CategoriaNombre { get; set; } = string.Empty;
    }
}
