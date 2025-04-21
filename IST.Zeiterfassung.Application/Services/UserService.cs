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

    public async Task<Result<User>> LoginAsync(LoginUserDTO dto)
    {
        var user = await _userRepository.GetByUsernameAsync(dto.Username);

        if (user is null)
            return Result<User>.Fail("Benutzer nicht gefunden.");

        if (!BCrypt.Net.BCrypt.Verify(dto.Passwort, user.PasswordHash))
            return Result<User>.Fail("Passwort falsch.");

        if (!user.Aktiv)
            return Result<User>.Fail("Benutzer ist deaktiviert.");

        return Result<User>.Ok(user);
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

    public async Task<Result<User>> LoginByNfcAsync(string uid)
    {
        var user = await _userRepository.GetByNfcUidAsync(uid);
        if (user == null)
            return Result<User>.Fail("Kein Benutzer mit dieser NFC-ID gefunden.");

        if (!user.Aktiv)
            return Result<User>.Fail("Benutzer ist deaktiviert.");

        return Result<User>.Ok(user);
    }

    public async Task<Result<User>> LoginByQrAsync(string token)
    {
        var user = await _userRepository.GetByQrTokenAsync(token);
        if (user == null)
            return Result<User>.Fail("Kein Benutzer mit diesem QR-Code gefunden.");

        if (!user.Aktiv)
            return Result<User>.Fail("Benutzer ist deaktiviert.");

        return Result<User>.Ok(user);
    }

    public async Task<User?> GetEntityByIdAsync(Guid id)
    {
        return await _userRepository.GetByIdAsync(id);
    }

    public async Task<List<UserListItemDTO>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();

        return users.Select(u => new UserListItemDTO
        {
            Id = u.Id,
            Username = u.Username,
            Email = u.Email,
            Role = u.Role.ToString(),
            Aktiv = u.Aktiv,
            ErstelltAm = u.ErstelltAm
        }).ToList();
    }

    public async Task<Result<string>> SetNfcIdAsync(Guid userId, SetNfcIdDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.NfcId))
            return Result<string>.Fail("Ungültige NFC-ID.");

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return Result<string>.Fail("Benutzer nicht gefunden.");

        user.NfcId = dto.NfcId.Trim();
        await _userRepository.UpdateAsync(user);

        return Result<string>.Ok("NFC-ID erfolgreich gespeichert.");
    }

    public async Task<Result<string>> SetQrTokenAsync(Guid userId, SetQrTokenDTO dto)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return Result<string>.Fail("Benutzer nicht gefunden.");

        if (dto.NeuGenerieren)
        {
            user.QrToken = Guid.NewGuid().ToString();
            user.QrTokenExpiresAt = DateTime.UtcNow.AddMinutes(10); // z. B. 10 Minuten gültig
        }
        else
        {
            user.QrToken = null;
            user.QrTokenExpiresAt = null;
        }

        await _userRepository.UpdateAsync(user);

        return Result<string>.Ok(dto.NeuGenerieren
            ? $"Neues QR-Token gesetzt. Gültig bis {user.QrTokenExpiresAt:HH:mm}."
            : "QR-Token wurde entfernt.");
    }


}
