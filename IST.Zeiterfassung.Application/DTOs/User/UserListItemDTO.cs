﻿namespace IST.Zeiterfassung.Application.DTOs.User;

public class UserListItemDTO
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = "Mitarbeiter";
    public bool Aktiv { get; set; }
    public Guid? ZeitmodellId { get; set; }
    public DateTime ErstelltAm { get; set; }

    public string? Name { get; set; }
    public string? LastName { get; set; }
    public string? EmployeeNumber { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Abteilung { get; set; }
    public string? Telefon { get; set; }
    public string? Standort { get; set; }




}
