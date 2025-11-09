// Spendnt.Shared/Entities/User.cs
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Spendnt.Shared.Entities
{
    public class User : IdentityUser
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;        public string FullName => $"{FirstName} {LastName}";

       
        [MaxLength(500)] 
        public string? ProfilePictureUrl { get; set; } 

        [JsonIgnore]
        public virtual Saldo? SaldoPrincipal { get; set; }

        [JsonIgnore]
        public virtual ICollection<MetaAhorro> MetasAhorro { get; set; } = new List<MetaAhorro>();

        [JsonIgnore]
        public virtual ICollection<RecordatorioGasto> RecordatoriosGasto { get; set; } = new List<RecordatorioGasto>();
    }
}