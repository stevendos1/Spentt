// Spendnt.API/Application/Interfaces/IHistorialService.cs
using Spendnt.Shared.DTOs.Historial;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spendnt.API.Application.Interfaces
{
    public interface IHistorialService
    {
        Task<IEnumerable<HistorialDto>> GetAsync(string userId);
        Task<HistorialDto?> GetByIdAsync(string userId, int id);
        Task<HistorialDto> CreateAsync(string userId, HistorialCreateDto dto);
        Task<bool> UpdateAsync(string userId, int id, HistorialUpdateDto dto);
        Task<bool> DeleteAsync(string userId, int id);
    }
}
