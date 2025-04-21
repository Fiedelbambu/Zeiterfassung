using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IST.Zeiterfassung.Domain.Entities;
using Microsoft.Extensions.Options;

namespace IST.Zeiterfassung.Application.Interfaces
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<List<User>> GetAllAsync();
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByIdAsync(Guid id);
        Task UpdateAsync(User user);
        Task<User?> GetByNfcUidAsync(string uid);
        Task<User?> GetByQrTokenAsync(string token);

    }
}
