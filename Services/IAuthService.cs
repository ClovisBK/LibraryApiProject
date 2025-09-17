using LibrarySystemApi.Dtos.AuthoDtos;
using LibrarySystemApi.Models;

namespace LibrarySystemApi.Services
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(RegisterUserDto request, string role);
        Task<TokenResponseDto?> LoginAsync(LoginDto request);
        Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request);
    }
}
