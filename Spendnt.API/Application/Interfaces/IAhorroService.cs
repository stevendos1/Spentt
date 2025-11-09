using Spendnt.Shared.Responses;
using System.Threading.Tasks;

namespace Spendnt.API.Application.Interfaces
{
    public interface IAhorroService
    {
        Task<OperationResponse> ContribuirAMeta(int metaId, decimal monto, string userId);
    }
}
