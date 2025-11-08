namespace FitnessCenter.Models
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        public string LastName { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string? MiddleName { get; set; }
        public string Position { get; set; } = default!;
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public DateTime HireDate { get; set; } = DateTime.Now;
        public bool IsTrainer { get; set; } = false;
    }
}
