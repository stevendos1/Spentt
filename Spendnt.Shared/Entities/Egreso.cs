// Spendnt.Shared/Entities/Egresos.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Spendnt.Shared.Entities
{
    public class Egresos
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Egreso")]
        [Column(TypeName = "decimal(18,2)")] 
        [Range(0.01, double.MaxValue)]
        public decimal Egreso { get; set; }

        [Required]
        [Display(Name = "Fecha")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "Por favor, seleccione una categoría.")]
        [Display(Name = "Categoría")]
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