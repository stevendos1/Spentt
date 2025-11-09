// Spendnt.API/Application/Interfaces/IIngresoService.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using Spendnt.Shared.DTOs.Ingreso;

namespace Spendnt.API.Application.Interfaces
{
    public interface IIngresoService
    {
        Task<IEnumerable<IngresoDto>> GetAsync(string userId);
        Task<IngresoDto?> GetByIdAsync(string userId, int id);
        Task<IngresoDto> CreateAsync(string userId, IngresoCreateDto dto);
        Task<bool> UpdateAsync(string userId, int id, IngresoUpdateDto dto);
        Task<bool> DeleteAsync(string userId, int id);
    }
}
