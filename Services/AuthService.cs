using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using TaskManagementApi.Data;
using TaskManagementApi.DTOs;
using TaskManagementApi.Models;
using TaskManagementApi.Services.Interfaces;
using TaskManagementApi.Mappings;

namespace TaskManagementApi.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthService> _logger;
    private readonly PasswordHasher<User> _passwordHasher;

    public AuthService(AppDbContext context, ITokenService tokenService, ILogger<AuthService> logger)
    {
        _context = context;
        _tokenService = tokenService;
        _logger = logger;
        _passwordHasher = new PasswordHasher<User>();
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        var emailExists = await _context.Users.AsNoTracking().AnyAsync(u => u.Email == dto.Email);
        if (emailExists) throw new InvalidOperationException("Email is already in use.");

        var userNameExists = await _context.Users.AsNoTracking().AnyAsync(u => u.UserName == dto.UserName);
        if (userNameExists) throw new InvalidOperationException("Username is already in use.");

        var user = new User
        {
            UserName = dto.UserName,
            Email = dto.Email,
            Role = "user"
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        _logger.LogInformation(
            "New user registered. Id: {UserId}, Email: {Email}, Role: {Role}",
            user.Id,
            user.Email,
            user.Role);

        return AuthMapper.ToResponseDto(user, _tokenService.GenerateToken(user));
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user is null) throw new UnauthorizedAccessException("Invalid email or password.");

        var verificationResult = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            dto.Password);

        if (verificationResult == PasswordVerificationResult.Failed) throw new UnauthorizedAccessException("Invalid email or password.");

        _logger.LogInformation(
            "User logged in. Id: {UserId}, Email: {Email}, Role: {Role}",
            user.Id,
            user.Email,
            user.Role);

        return AuthMapper.ToResponseDto(user, _tokenService.GenerateToken(user));
    }
}