// Spendnt.Shared/DTOs/RecordatorioGasto/RecordatorioGastoDto.cs
using System;

#nullable enable

namespace Spendnt.Shared.DTOs.RecordatorioGasto
{
    public class RecordatorioGastoDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public decimal MontoEstimado { get; set; }
        public DateTime FechaProgramada { get; set; }
        public string? Notas { get; set; }
    }
}
