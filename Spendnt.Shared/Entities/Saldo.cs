// Spendnt.Shared/Entities/Saldo.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // Para [Required]
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;

namespace Spendnt.Shared.Entities
{
    public class Saldo
    {
        private decimal? _totalSaldoManual;

        public int Id { get; set; }

        [JsonIgnore]
        public virtual ICollection<Ingresos> Ingresos { get; set; } // Añadido virtual
        [JsonIgnore]
        public virtual ICollection<Egresos> Egresos { get; set; } // Añadido virtual

        [NotMapped]
        public decimal TotalIngresos { get; set; }
        [NotMapped]
        public decimal TotalEgresos { get; set; }
        [NotMapped]
        public decimal TotalSaldoCalculado { get; set; }

        [NotMapped]
        public decimal TotalSaldo
        {
            get => _totalSaldoManual ?? TotalSaldoCalculado;
            set => _totalSaldoManual = value;
        }

      
      
        [Required]
        public string UserId { get; set; } = string.Empty;

        [JsonIgnore]
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }        public Saldo()
        {
            Ingresos = new List<Ingresos>();
            Egresos = new List<Egresos>();
        }
    }
}