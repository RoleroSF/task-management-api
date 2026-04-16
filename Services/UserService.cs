using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using TaskManagementApi.Data;
using TaskManagementApi.DTOs;
using TaskManagementApi.Models;
using TaskManagementApi.Services.Interfaces;
using TaskManagementApi.Helpers;
using TaskManagementApi.Mappings;

namespace TaskManagementApi.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _context;
    private readonly PasswordHasher<User> _passwordHasher;
    public UserService(AppDbContext context)
    {
        _context = context;
        _passwordHasher = new PasswordHasher<User>();
    }
    public async Task<UserResponseDto> GetUserByIdAsync(int id)
    {
        var user = await _context.Users
            .AsNoTracking()
            .Where(u => u.Id == id)
            .Select(UserMapper.ToResponseDtoExpression)
            .FirstOrDefaultAsync();

        if (user == null) throw new KeyNotFoundException("User not found.");

        return user;
    }
    public async Task<UserResponseDto> GetUserByEmailAsync(string email)
    {
        var user = await _context.Users
            .AsNoTracking()
            .Where(u => u.Email == email)
            .Select(UserMapper.ToResponseDtoExpression)
            .FirstOrDefaultAsync();
        if (user == null) throw new KeyNotFoundException("User not found.");
        return user;
    }
    public async Task<IList<UserResponseDto>> GetAllUsersAsync()
    {
        var users = await _context.Users
            .AsNoTracking()
            .Select(UserMapper.ToResponseDtoExpression)
            .ToListAsync();

        return users;
    }
    public async Task<UserResponseDto> UpdateUserAsync(int id, UpdateUserDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null) throw new KeyNotFoundException("User not found.");

        if (!string.IsNullOrWhiteSpace(dto.UserName))
        {
            var nameExists = await _context.Users.AsNoTracking().AnyAsync(u => u.UserName == dto.UserName && u.Id != user.Id);
            if (nameExists) throw new InvalidOperationException("UserName is already in use by another user.");

            user.UserName = dto.UserName.ToLower();
        }

        if (!string.IsNullOrWhiteSpace(dto.Email))
        {
            var emailExists = await _context.Users.AsNoTracking().AnyAsync(u => u.Email == dto.Email && u.Id != user.Id);
            if (emailExists) throw new InvalidOperationException("Email is already in use by another user.");

            user.Email = dto.Email.ToLower();
        }
        if (!string.IsNullOrWhiteSpace(dto.Role))
        {
            if (!UserHelper.IsAllowedRoles(dto.Role))
                throw new InvalidOperationException("Invalid role value.");

            user.Role = dto.Role.ToLower();
        } 
        if (!string.IsNullOrWhiteSpace(dto.Password)) user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

        await _context.SaveChangesAsync();

        return UserMapper.ToResponseDto(user);
    }
}