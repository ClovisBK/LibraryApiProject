using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using LibrarySystemApi.Data;
using LibrarySystemApi.Dtos.AuthoDtos;
using LibrarySystemApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LibrarySystemApi.Services
{
    public class AuthService(BookLibraryDbContext context, IConfiguration configuration) : IAuthService
    {
        private readonly BookLibraryDbContext _context = context;
        private readonly IConfiguration _configuration = configuration;

        //-------------------------------REGISTER-----------------------------------
        public async Task<User?> RegisterAsync(RegisterUserDto request, string role)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                return null;

            
            var user = new User
            {
                Email = request.Email,
                Role = role
            };
            user.PasswordHash = new PasswordHasher<User>()
                .HashPassword(user, request.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            if(role == "Member")
            {
                var member = new Member
                {
                    UserId = user.Id,
                    Email = request.Email,
                    FullName = request.FullName,
                    Phone = request.Phone,
                    JoinedDate = DateTime.UtcNow
                };
                await _context.Members.AddAsync(member);
                await _context.SaveChangesAsync();
            }
            else if(role == "Admin")
            {
                var admin = new Admin
                {
                    UserId = user.Id,
                    Email = request.Email,
                    Phone = request.Phone,
                    FullName = request.FullName,
                    JoinedDate = DateTime.UtcNow
                };
                await _context.Admins.AddAsync(admin);
                await _context.SaveChangesAsync();
            }

            return user;
        }



        //---------------------LOGIN---------------------------------
        public async Task<TokenResponseDto?> LoginAsync(LoginDto request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null) 
                return null;
            var result = new PasswordHasher<User>()
                .VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if(result == PasswordVerificationResult.Failed)
                return null;
            return await CreateTokenResponse(user);
               
                
        }

        //------------------REFRESH TOKEN-------------------
        public async Task<User?> ValidateRefreshTokenAsync(Guid userId, string refreshToken)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow) 
                return null;
            return user;
        }

        public async Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request)
        {
            var user = await ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);
            if (user == null)
                return null;
            return await CreateTokenResponse(user);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
        {
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _context.SaveChangesAsync();


            return refreshToken;
        }

        //-------------------------------------JWT TOKEN--------------------------------

        public async Task<TokenResponseDto> CreateTokenResponse(User user)
        {
            return new TokenResponseDto
            {
                AccessToken = CreateToken(user),
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
            };
        }


        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)

            };
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetValue<string>("AppSettings:Token")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("AppSettings:Issuer"),
                audience: _configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds

                );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
