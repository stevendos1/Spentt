// Spendnt.Shared/DTOs/Saldo/SaldoMovimientoDto.cs
using System;

#nullable enable

namespace Spendnt.Shared.DTOs.Saldo
{
    public class SaldoMovimientoDto
    {
        public int Id { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public string? CategoriaNombre { get; set; }
    }
}
