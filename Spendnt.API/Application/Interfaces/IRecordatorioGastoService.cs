// Spendnt.API/Application/Interfaces/IRecordatorioGastoService.cs
using Spendnt.Shared.DTOs.RecordatorioGasto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spendnt.API.Application.Interfaces
{
    public interface IRecordatorioGastoService
    {
        Task<IEnumerable<RecordatorioGastoDto>> GetAsync(string userId);
        Task<RecordatorioGastoDto?> GetByIdAsync(string userId, int id);
        Task<RecordatorioGastoDto> CreateAsync(string userId, RecordatorioGastoCreateDto dto);
        Task<bool> UpdateAsync(string userId, int id, RecordatorioGastoUpdateDto dto);
        Task<bool> DeleteAsync(string userId, int id);
    }
}
