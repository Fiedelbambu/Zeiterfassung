using IST.Zeiterfassung.Application.DTOs.User;
using IST.Zeiterfassung.Application.Interfaces;
using IST.Zeiterfassung.Application.Results;
using IST.Zeiterfassung.Domain.Entities;
using IST.Zeiterfassung.Domain.Enums;
using IST.Zeiterfassung.Application.Interfaces;

using BCrypt.Net;
using Microsoft.EntityFrameworkCore;


namespace IST.Zeiterfassung.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ILoginAuditRepository _auditRepository;
    private readonly IZeitmodellRepository _zeitmodellRepo;

    public UserService(IUserRepository userRepository, IZeitmodellRepository zeitmodellRepo, ILoginAuditRepository auditRepository)
    {
        _userRepository = userRepository;
        _auditRepository = auditRepository;
        _zeitmodellRepo = zeitmodellRepo;
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
            Role = user.Role.ToString()
        };

        return Result<UserResponseDTO>.Ok(response);
    }

    public async Task<Result<User>> LoginAsync(LoginUserDTO dto, HttpContext context)
    {
        var user = await _userRepository.GetByUsernameAsync(dto.Username);

        var audit = new LoginAudit
        {
            LoginMethod = LoginMethod.Passwort,
            Zeitpunkt = DateTime.UtcNow,
            IPAdresse = context.Connection?.RemoteIpAddress?.ToString(),
            UserId = user?.Id,
            Erfolgreich = false,
            Ort = null
        };

        if (user is null)
        {
            audit.Ort = "Benutzer nicht gefunden";
            await _auditRepository.AddAsync(audit);
            return Result<User>.Fail("Benutzer nicht gefunden.");
        }
        try
        {
            if (!BCrypt.Net.BCrypt.Verify(dto.Passwort, user.PasswordHash))
            {
                audit.Ort = "Passwort falsch";
                await _auditRepository.AddAsync(audit);
                return Result<User>.Fail("Passwort falsch.");
            }
        }
        catch (Exception ex)
        {
            audit.Ort = "Fehler beim Passwortvergleich";
            await _auditRepository.AddAsync(audit);
         //   _logger.LogError(ex, "Fehler beim Passwortvergleich");
            return Result<User>.Fail("Interner Fehler bei der Anmeldung.");
        }
        if (!user.Aktiv)
        {
            audit.Ort = "Benutzer ist deaktiviert";
            await _auditRepository.AddAsync(audit);
            return Result<User>.Fail("Benutzer ist deaktiviert.");
        }

        audit.Erfolgreich = true;
        audit.Ort = "Login erfolgreich";
        await _auditRepository.AddAsync(audit);

        return Result<User>.Ok(user);
    }

    public async Task<List<UserResponseDTO>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();

        return users.Select(u => new UserResponseDTO
        {
            Id = u.Id,
            Username = u.Username,
            LastName = u.LastName,
            BirthDate = u.BirthDate,
            Email = u.Email,
            EmployeeNumber = u.EmployeeNumber,
            Role = u.Role.ToString(),
            IsActive = u.Aktiv
        }).ToList();
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

    public async Task<Result<User>> LoginByNfcAsync(string uid, HttpContext context)
    {
        var user = await _userRepository.GetByNfcUidAsync(uid);

        var audit = new LoginAudit
        {
            LoginMethod = LoginMethod.NFC,
            IPAdresse = context.Connection?.RemoteIpAddress?.ToString(),
            Zeitpunkt = DateTime.UtcNow,
            Erfolgreich = user != null && user.Aktiv,
            UserId = user?.Id,
            Ort = user == null ? "UID nicht bekannt" : (user.Aktiv ? "Erfolg" : "Benutzer deaktiviert")
        };

        await _auditRepository.AddAsync(audit);

        if (user == null || !user.Aktiv)
            return Result<User>.Fail("Login über NFC nicht möglich.");

        return Result<User>.Ok(user);
    }

    public async Task<Result<User>> LoginByQrAsync(string token, HttpContext context)
    {
        var user = await _userRepository.GetByQrTokenAsync(token);

        var audit = new LoginAudit
        {
            LoginMethod = LoginMethod.QRCode,
            Zeitpunkt = DateTime.UtcNow,
            IPAdresse = context.Connection?.RemoteIpAddress?.ToString(),
            UserId = user?.Id,
            Erfolgreich = user != null && user.Aktiv,
            Ort = user == null ? "QR-Token ungültig" : (!user.Aktiv ? "Benutzer deaktiviert" : "Login via QR erfolgreich")
        };

        await _auditRepository.AddAsync(audit);

        if (user == null || !user.Aktiv)
            return Result<User>.Fail("Login über QR nicht möglich.");

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
    public async Task<Result<string>> SetZeitmodellAsync(Guid userId, SetZeitmodellDTO dto)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
            return Result<string>.Fail("Benutzer nicht gefunden.");

        var modell = await _zeitmodellRepo.GetByIdAsync(dto.ZeitmodellId);
        if (modell is null)
            return Result<string>.Fail("Zeitmodell nicht gefunden.");

        user.ZeitmodellId = dto.ZeitmodellId;
        await _userRepository.UpdateAsync(user);

        return Result<string>.Ok("Zeitmodell wurde zugewiesen.");
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
            LastName = user.LastName,
            Email = user.Email,
            BirthDate = user.BirthDate,
            EmployeeNumber = user.EmployeeNumber,
            Role = user.Role.ToString(),
            IsActive = user.Aktiv
        });
    }
    public async Task<Result<UserResponseDTO>> CreateAsync(CreateUserDTO dto)
    {
        var existingUser = await _userRepository.GetByUsernameAsync(dto.Name);
        if (existingUser != null)
        {
            return Result<UserResponseDTO>.Fail("Benutzername ist bereits vergeben.");
        }

        var newUser = new User
        {
            Id = Guid.NewGuid(),
            Username = dto.Name,
            Name = dto.Name,
            LastName = dto.LastName,
            BirthDate = dto.BirthDate,
            EmployeeNumber = dto.EmployeeNumber,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = Enum.TryParse<Role>(dto.Role, out var parsedRole) ? parsedRole : Role.Employee,
            Aktiv = dto.IsActive,
            ErstelltAm = DateTime.UtcNow
        };

        await _userRepository.AddAsync(newUser);

        return Result<UserResponseDTO>.Ok(new UserResponseDTO
        {
            Id = newUser.Id,
            Username = newUser.Username,
            LastName = newUser.LastName,
            BirthDate = newUser.BirthDate,
            Email = newUser.Email,
            EmployeeNumber = newUser.EmployeeNumber,
            Role = newUser.Role.ToString(),
            IsActive = newUser.Aktiv
        });
    }


}
