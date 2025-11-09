// Spendnt.API/Application/Interfaces/ITransaccionAhorroService.cs
using Spendnt.Shared.DTOs.TransaccionAhorro;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spendnt.API.Application.Interfaces
{
    public interface ITransaccionAhorroService
    {
        Task<IEnumerable<TransaccionAhorroDto>?> GetAsync(string userId, int metaAhorroId);
        Task<TransaccionAhorroDto?> CreateAsync(string userId, int metaAhorroId, TransaccionAhorroCreateDto dto);
    }
}
