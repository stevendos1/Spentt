// Spendnt.Shared/Entities/RecordatorioGasto.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace Spendnt.Shared.Entities
{
    public class RecordatorioGasto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es obligatorio.")]
        [MaxLength(200, ErrorMessage = "El título no puede exceder los 200 caracteres.")]
        public string Titulo { get; set; } = string.Empty;        [Required(ErrorMessage = "El monto estimado es obligatorio.")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto estimado debe ser mayor a cero.")]
        public decimal MontoEstimado { get; set; }

        [Required(ErrorMessage = "La fecha programada es obligatoria.")]
        [DataType(DataType.Date)] 
        public DateTime FechaProgramada { get; set; }

        [MaxLength(500, ErrorMessage = "Las notas no pueden exceder los 500 caracteres.")]
    public string? Notas { get; set; }

        
        [Required]
        public string UserId { get; set; } = string.Empty;

        [JsonIgnore]
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
    }
}