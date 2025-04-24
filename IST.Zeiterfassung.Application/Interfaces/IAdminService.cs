using IST.Zeiterfassung.Application.DTOs.Admin;

public interface IAdminService
{
    Task<SystemStatusDTO> GetSystemStatusAsync();
}
