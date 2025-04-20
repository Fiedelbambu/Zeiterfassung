using IST.Zeiterfassung.Application.Interfaces;
using IST.Zeiterfassung.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IST.Zeiterfassung.Persistence.Repositories;

public class TimeModelRepository : ITimeModelRepository
{
    private readonly AppDbContext _context;

    public TimeModelRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Zeitmodell?> GetByUserIdAsync(Guid userId)
    {
        return await _context.Zeitmodelle
            .FirstOrDefaultAsync(z => z.Id == userId); // Alternativ: z.UserId == userId
    }

    public async Task SetOrUpdateAsync(Guid userId, Zeitmodell modell)
    {
        var existing = await _context.Zeitmodelle
            .FirstOrDefaultAsync(z => z.Id == userId); // Alternativ: z.UserId == userId

        if (existing is null)
        {
            modell.Id = userId; // alternativ: modell.UserId = userId;
            _context.Zeitmodelle.Add(modell);
        }
        else
        {
            existing.Bezeichnung = modell.Bezeichnung;
            existing.WochenSollzeit = modell.WochenSollzeit;
            existing.IstGleitzeit = modell.IstGleitzeit;
            existing.SollzeitProTag = modell.SollzeitProTag;
            existing.GleitzeitkontoAktiv = modell.GleitzeitkontoAktiv;
            existing.GleitzeitMonatslimit = modell.GleitzeitMonatslimit;
            existing.SaldoÜbertragAktiv = modell.SaldoÜbertragAktiv;

            _context.Zeitmodelle.Update(existing);
        }

        await _context.SaveChangesAsync();
    }
}
