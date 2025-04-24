using IST.Zeiterfassung.Application.Interfaces;
using IST.Zeiterfassung.Domain.Entities;

namespace IST.Zeiterfassung.Persistence
{
    public class ZeitmodellRepository : IZeitmodellRepository
    {
        private readonly AppDbContext _context;

        public ZeitmodellRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Zeitmodell?> GetByIdAsync(Guid id)
        {
            return await _context.Zeitmodelle.FindAsync(id);
        }
    }
}
