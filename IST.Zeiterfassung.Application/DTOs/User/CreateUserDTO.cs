
namespace IST.Zeiterfassung.Application.DTOs.User
{
    public class CreateUserDTO
    {
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = "Employee";
        public string Password { get; set; } = string.Empty;
        public string EmployeeNumber { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
