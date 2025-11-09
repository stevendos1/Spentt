// Spendnt.Shared/DTOs/Saldo/SaldoDto.cs
using System.Collections.Generic;

#nullable enable

namespace Spendnt.Shared.DTOs.Saldo
{
    public class SaldoDto
    {
        public int Id { get; set; }
        public decimal TotalSaldo { get; set; }
        public decimal TotalIngresos { get; set; }
        public decimal TotalEgresos { get; set; }
        public decimal TotalSaldoCalculado { get; set; }
        public IEnumerable<SaldoMovimientoDto> Ingresos { get; set; } = new List<SaldoMovimientoDto>();
        public IEnumerable<SaldoMovimientoDto> Egresos { get; set; } = new List<SaldoMovimientoDto>();
    }
}
