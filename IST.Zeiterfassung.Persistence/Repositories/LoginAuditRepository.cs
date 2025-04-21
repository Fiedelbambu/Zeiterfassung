using IST.Zeiterfassung.Application.Interfaces;
using IST.Zeiterfassung.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IST.Zeiterfassung.Persistence.Repositories
{
    public class LoginAuditRepository : ILoginAuditRepository
    {
        private readonly AppDbContext _context;

        public LoginAuditRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(LoginAudit eintrag)
        {
            await _context.LoginAudits.AddAsync(eintrag);
            await _context.SaveChangesAsync();
        }
    }
}
