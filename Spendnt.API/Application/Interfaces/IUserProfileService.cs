// Spendnt.API/Application/Interfaces/IUserProfileService.cs
using Microsoft.AspNetCore.Http;
using Spendnt.Shared.DTOs;
using System.Threading.Tasks;

#nullable enable

namespace Spendnt.API.Application.Interfaces
{
    public interface IUserProfileService
    {
        Task<UserViewModel?> GetUserProfileAsync(string userId);
        Task<bool> UpdateUserProfileAsync(string userId, UserProfileUpdateDto model);
        Task<string> UploadProfilePictureAsync(string userId, IFormFile file);
    }
}
