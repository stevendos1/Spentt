// Spendnt.API/Application/Interfaces/ISaldoService.cs
using Spendnt.Shared.DTOs.Saldo;
using System.Threading.Tasks;

namespace Spendnt.API.Application.Interfaces
{
    public interface ISaldoService
    {
        Task<SaldoDto?> GetCurrentAsync(string userId);
        Task<SaldoDto?> GetByIdAsync(string userId, int id);
        Task<SaldoDto?> UpdateAsync(string userId, int id, SaldoUpdateDto dto);
    }
}
