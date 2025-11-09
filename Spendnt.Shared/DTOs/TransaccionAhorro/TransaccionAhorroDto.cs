// Spendnt.Shared/DTOs/TransaccionAhorro/TransaccionAhorroDto.cs
using System;

#nullable enable

namespace Spendnt.Shared.DTOs.TransaccionAhorro
{
    public class TransaccionAhorroDto
    {
        public int Id { get; set; }
        public int MetaAhorroId { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public string? Descripcion { get; set; }
    }
}
