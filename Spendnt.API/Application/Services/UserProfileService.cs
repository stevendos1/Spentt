// Spendnt.API/Application/Services/UserProfileService.cs
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Spendnt.API.Application.Interfaces;
using Spendnt.API.Helpers;
using Spendnt.Shared.DTOs;
using Spendnt.Shared.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

#nullable enable

namespace Spendnt.API.Application.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly UserManager<User> _userManager;
        private readonly IFileStorage _fileStorage;
        private readonly ILogger<UserProfileService> _logger;

        public UserProfileService(UserManager<User> userManager, IFileStorage fileStorage, ILogger<UserProfileService> logger)
        {
            _userManager = userManager;
            _fileStorage = fileStorage;
            _logger = logger;
        }

        public async Task<UserViewModel?> GetUserProfileAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return null;
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return null;
            }

            var roles = await _userManager.GetRolesAsync(user) ?? new List<string>();

            return new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ProfilePictureUrl = user.ProfilePictureUrl,
                Roles = roles
            };
        }

        public async Task<bool> UpdateUserProfileAsync(string userId, UserProfileUpdateDto model)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return false;
            }

            ArgumentNullException.ThrowIfNull(model);

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            user.FirstName = model.FirstName ?? user.FirstName;
            user.LastName = model.LastName ?? user.LastName;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<string> UploadProfilePictureAsync(string userId, IFormFile file)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("El identificador de usuario es obligatorio.", nameof(userId));
            }

            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("El archivo no puede ser nulo o estar vacío.", nameof(file));
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException("Usuario no encontrado.");
            }

            if (!string.IsNullOrEmpty(user.ProfilePictureUrl))
            {
                try
                {
                    await _fileStorage.DeleteFileAsync(user.ProfilePictureUrl, "profile_pictures");
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "No se pudo eliminar la foto de perfil anterior para el usuario {UserId}", userId);
                }
            }

            string fileUrl;
            try
            {
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                var content = memoryStream.ToArray();
                var extension = Path.GetExtension(file.FileName);
                fileUrl = await _fileStorage.SaveFileAsync(content, extension, "profile_pictures");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar la foto de perfil para el usuario {UserId}", userId);
                throw;
            }

            user.ProfilePictureUrl = fileUrl;
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                try
                {
                    await _fileStorage.DeleteFileAsync(fileUrl, "profile_pictures");
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Falló la eliminación del archivo tras la actualización fallida del usuario {UserId}", userId);
                }

                throw new InvalidOperationException("No se pudo actualizar el usuario después de subir la foto de perfil.");
            }

            return fileUrl;
        }
    }
}
