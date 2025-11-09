// Spendnt.Shared/DTOs/UserProfileUpdateDto.cs
using System.ComponentModel.DataAnnotations;

#nullable enable

namespace Spendnt.Shared.DTOs
{
    public class UserProfileUpdateDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;
    }
}