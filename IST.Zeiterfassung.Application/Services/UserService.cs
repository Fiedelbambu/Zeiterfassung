using IST.Zeiterfassung.Application.DTOs.User;
using IST.Zeiterfassung.Application.Interfaces;
using IST.Zeiterfassung.Application.Results;
using IST.Zeiterfassung.Domain.Entities;
using IST.Zeiterfassung.Domain.Enums;

using BCrypt.Net;


namespace IST.Zeiterfassung.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<UserResponseDTO>> RegisterAsync(RegisterUserDTO dto)
    {
        var existingUser = await _userRepository.GetByUsernameAsync(dto.Username);
        if (existingUser is not null)
            return Result<UserResponseDTO>.Fail("Benutzername ist bereits vergeben.");

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Passwort);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = passwordHash,
            Role = Role.Employee,
            Aktiv = true,
            ErstelltAm = DateTime.UtcNow
        };

        await _userRepository.AddAsync(user);

        var response = new UserResponseDTO
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role
        };

        return Result<UserResponseDTO>.Ok(response);
    }

    public async Task<Result<UserResponseDTO>> LoginAsync(LoginUserDTO dto)
    {
        var user = await _userRepository.GetByUsernameAsync(dto.Username);

        if (user is null)
            return Result<UserResponseDTO>.Fail("Benutzer nicht gefunden.");

        if (!BCrypt.Net.BCrypt.Verify(dto.Passwort, user.PasswordHash))
            return Result<UserResponseDTO>.Fail("Passwort falsch.");

        if (!user.Aktiv)
            return Result<UserResponseDTO>.Fail("Benutzer ist deaktiviert.");

        var response = new UserResponseDTO
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role
        };

        return Result<UserResponseDTO>.Ok(response);
    }

    public async Task<List<UserResponseDTO>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();

        return users.Select(u => new UserResponseDTO
        {
            Id = u.Id,
            Username = u.Username,
            Email = u.Email,
            Role = u.Role
        }).ToList();
    }

    public async Task<Result<UserResponseDTO>> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user is null)
            return Result<UserResponseDTO>.Fail("Benutzer nicht gefunden.");

        return Result<UserResponseDTO>.Ok(new UserResponseDTO
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role
        });
    }

    public async Task<Result<string>> ChangePasswordAsync(Guid userId, ChangePasswordDTO dto)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
            return Result<string>.Fail("Benutzer nicht gefunden.");

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NeuesPasswort);
        await _userRepository.UpdateAsync(user);

        return Result<string>.Ok("Passwort erfolgreich geändert.");
    }

    public async Task<Result<string>> ChangeRoleAsync(Guid userId, ChangeUserRoleDTO dto)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
            return Result<string>.Fail("Benutzer nicht gefunden.");

        user.Role = dto.NeueRolle;
        await _userRepository.UpdateAsync(user);

        return Result<string>.Ok("Benutzerrolle erfolgreich geändert.");
    }

    public async Task<Result<string>> ToggleAktivAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
            return Result<string>.Fail("Benutzer nicht gefunden.");

        user.Aktiv = !user.Aktiv;
        await _userRepository.UpdateAsync(user);

        var status = user.Aktiv ? "aktiviert" : "deaktiviert";
        return Result<string>.Ok($"Benutzer wurde {status}.");
    }


}
