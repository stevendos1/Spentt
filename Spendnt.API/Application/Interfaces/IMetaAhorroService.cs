// Spendnt.API/Application/Interfaces/IMetaAhorroService.cs
using Spendnt.Shared.DTOs.MetaAhorro;
using Spendnt.Shared.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spendnt.API.Application.Interfaces
{
    public interface IMetaAhorroService
    {
        Task<IEnumerable<MetaAhorroDto>> GetAsync(string userId);
        Task<MetaAhorroDto?> GetByIdAsync(string userId, int id);
        Task<MetaAhorroDto> CreateAsync(string userId, MetaAhorroCreateDto dto);
        Task<bool> UpdateAsync(string userId, int id, MetaAhorroUpdateDto dto);
        Task<bool> DeleteAsync(string userId, int id);
        Task<OperationResponse> ContribuirAsync(int metaId, decimal monto, string userId);
    }
}
