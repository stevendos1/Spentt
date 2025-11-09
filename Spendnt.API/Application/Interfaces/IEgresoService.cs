// Spendnt.API/Application/Interfaces/IEgresoService.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using Spendnt.Shared.DTOs.Egreso;

namespace Spendnt.API.Application.Interfaces
{
    public interface IEgresoService
    {
        Task<IEnumerable<EgresoDto>> GetAsync(string userId);
        Task<EgresoDto?> GetByIdAsync(string userId, int id);
        Task<EgresoDto> CreateAsync(string userId, EgresoCreateDto dto);
        Task<bool> UpdateAsync(string userId, int id, EgresoUpdateDto dto);
        Task<bool> DeleteAsync(string userId, int id);
    }
}
