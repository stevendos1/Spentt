// Spendnt.Shared/Entities/Historial.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // Para ForeignKey y Column
using System.Text.Json.Serialization;

namespace Spendnt.Shared.Entities
{
    public class Historial
    {
        public int Id { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")] 
        public decimal Monto { get; set; }

        [Required]
        [MaxLength(50)]
        public string Tipo { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Descripcion { get; set; }

        [Required]
        public int CategoriaId { get; set; }
        [JsonIgnore]
        public virtual Categoria? Categoria { get; set; }

      
        [Required]
        public int SaldoId { get; set; } 

        [JsonIgnore]
        [ForeignKey("SaldoId")]
        public virtual Saldo? Saldo { get; set; }
    }
}