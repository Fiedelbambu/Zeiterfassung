﻿// src/Application/DTOs/User/UserResponseDTO.cs

namespace IST.Zeiterfassung.Application.DTOs.User
{
    public class UserResponseDTO
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime? BirthDate { get; set; }
        public string EmployeeNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime ErstelltAm { get; set; }

    }
}
