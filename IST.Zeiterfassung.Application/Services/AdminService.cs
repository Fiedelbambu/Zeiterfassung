using IST.Zeiterfassung.Application.DTOs.Admin;
using IST.Zeiterfassung.Application.Interfaces;

public class AdminService : IAdminService
{
    private readonly IUserRepository _userRepo;
    private readonly ITimeEntryRepository _timeRepo;
    private readonly IAbsenceRepository _absenceRepo;
    private readonly ILoginAuditRepository _auditRepo;

    public AdminService(IUserRepository userRepo, ITimeEntryRepository timeRepo,
        IAbsenceRepository absenceRepo, ILoginAuditRepository auditRepo)
    {
        _userRepo = userRepo;
        _timeRepo = timeRepo;
        _absenceRepo = absenceRepo;
        _auditRepo = auditRepo;
    }

    public async Task<SystemStatusDTO> GetSystemStatusAsync()
    {
        var users = await _userRepo.GetAllAsync();
        var audits = await _auditRepo.GetAllAsync();

        var letzterLogin = audits
            .Where(a => a.Erfolgreich)
            .OrderByDescending(a => a.Zeitpunkt)
            .FirstOrDefault()?.Zeitpunkt;

        return new SystemStatusDTO
        {
            GesamtBenutzer = users.Count,
            AktiveBenutzer = users.Count(u => u.Aktiv),
            LetzterLogin = letzterLogin,
            Zeitbuchungen = (await _timeRepo.GetAllByUserIdAsync(Guid.Empty)).Count, // wird ersetzt
            Abwesenheiten = (await _absenceRepo.GetAllAsync()).Count,
            LoginVersucheGesamt = audits.Count,
            LoginFehlversuche = audits.Count(a => !a.Erfolgreich)
        };
    }
}
