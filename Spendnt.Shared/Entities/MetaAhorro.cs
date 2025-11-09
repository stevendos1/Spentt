// Spendnt.Shared/Entities/MetaAhorro.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Spendnt.Shared.Entities
{
    public class MetaAhorro
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la meta es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        [Display(Name = "Nombre de la Meta")]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres.")]
        public string? Descripcion { get; set; }        [Required(ErrorMessage = "El monto objetivo es obligatorio.")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto objetivo debe ser mayor a cero.")]
        [Display(Name = "Monto Objetivo")]
        public decimal MontoObjetivo { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "El monto actual no puede ser negativo.")]
        [Display(Name = "Monto Actual Ahorrado")]
        public decimal MontoActual { get; set; }

        [Display(Name = "Fecha Objetivo")]
        [DataType(DataType.Date)]
        public DateTime? FechaObjetivo { get; set; }

        [Display(Name = "Fecha de Creación")]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        [Display(Name = "Completada")]
        public bool EstaCompletada { get; set; } = false;

      
        [Required]
        public string UserId { get; set; } = string.Empty;

        [JsonIgnore]
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
    }
}