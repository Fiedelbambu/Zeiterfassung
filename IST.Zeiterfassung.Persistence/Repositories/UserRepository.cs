using Microsoft.EntityFrameworkCore;
using IST.Zeiterfassung.Application.Interfaces;
using IST.Zeiterfassung.Domain.Entities;
using IST.Zeiterfassung.Persistence;

namespace IST.Zeiterfassung.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByNfcUidAsync(string uid)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.NfcId == uid);
    }

    public async Task<User?> GetByQrTokenAsync(string token)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.QrToken == token);
    }



    public async Task AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
}
