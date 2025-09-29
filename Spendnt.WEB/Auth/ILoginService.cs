
using System.Threading.Tasks; 

namespace Spendnt.WEB.Auth 
{
    public interface ILoginService
    {
        Task LoginAsync(string token);
        Task LogoutAsync();
    }
}