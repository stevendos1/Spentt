// Spendnt.Shared/Entities/TransaccionAhorro.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Spendnt.Shared.Entities
{
    public class TransaccionAhorro
    {
        public int Id { get; set; }

        [Required]
        public int MetaAhorroId { get; set; }

        [JsonIgnore]
        public MetaAhorro? MetaAhorro { get; set; }        [Required(ErrorMessage = "El monto de la transacción es obligatorio.")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Monto")]
        public decimal Monto { get; set; }

        [Required]
        [Display(Name = "Fecha")]
        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        [MaxLength(200)]
        public string Descripcion { get; set; } = string.Empty;
    }
}