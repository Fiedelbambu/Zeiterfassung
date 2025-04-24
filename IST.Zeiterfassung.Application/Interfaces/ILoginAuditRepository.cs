using IST.Zeiterfassung.Domain.Entities;

namespace IST.Zeiterfassung.Application.Interfaces
{
    public interface ILoginAuditRepository
    {
        Task AddAsync(LoginAudit eintrag);
        Task<List<LoginAudit>> GetAllAsync();

    }
}
