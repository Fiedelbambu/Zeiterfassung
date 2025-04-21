using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IST.Zeiterfassung.Application.DTOs.User;
using IST.Zeiterfassung.Application.Results;
using IST.Zeiterfassung.Domain.Entities;
using IST.Zeiterfassung.Domain.Enums;

namespace IST.Zeiterfassung.Application.Interfaces
{
    public interface IUserService
    {
        Task<Result<UserResponseDTO>> RegisterAsync(RegisterUserDTO dto);

        //    Task<Result<UserResponseDTO>> LoginAsync(LoginUserDTO dto);
        Task<Result<User>> LoginAsync(LoginUserDTO dto);
        Task<List<UserResponseDTO>> GetAllAsync();
        Task<Result<UserResponseDTO>> GetByIdAsync(Guid id);
        Task<Result<string>> ChangePasswordAsync(Guid userId, ChangePasswordDTO dto);
        Task<Result<string>> ChangeRoleAsync(Guid userId, ChangeUserRoleDTO dto);
        Task<Result<string>> ToggleAktivAsync(Guid userId);
        Task<Result<User>> LoginByNfcAsync(string uid);
        Task<Result<User>> LoginByQrAsync(string token);
        Task<List<UserListItemDTO>> GetAllUsersAsync();
        Task<Result<string>> SetNfcIdAsync(Guid userId, SetNfcIdDTO dto);
        Task<Result<string>> SetQrTokenAsync(Guid userId, SetQrTokenDTO dto);


    }
}
