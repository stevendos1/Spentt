// Spendnt.Shared/DTOs/MetaAhorro/MetaAhorroDto.cs
using System;

#nullable enable

namespace Spendnt.Shared.DTOs.MetaAhorro
{
    public class MetaAhorroDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public decimal MontoObjetivo { get; set; }
        public decimal MontoActual { get; set; }
        public DateTime? FechaObjetivo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool EstaCompletada { get; set; }
    }
}
