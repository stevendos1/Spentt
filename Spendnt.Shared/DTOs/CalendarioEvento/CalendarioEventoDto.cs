using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spendnt.Shared.DTOs.CalendarioEvento
{
    public class CalendarioEventoDto
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public decimal? Monto { get; set; }
        public string Color { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }
}
