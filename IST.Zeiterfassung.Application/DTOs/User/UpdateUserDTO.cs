namespace IST.Zeiterfassung.Application.DTOs.User
{
    public class UpdateUserDTO
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string BirthDate { get; set; }
        public string EmployeeNumber { get; set; }
        public bool Aktiv { get; set; }
    }
}
