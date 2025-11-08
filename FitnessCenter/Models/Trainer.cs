namespace FitnessCenter.Models
{
    public class Trainer
    {
        public int TrainerID { get; set; }
        public string LastName { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string? MiddleName { get; set; }
        public string Specialty { get; set; } = default!;
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string Schedule { get; set; } = "Пн-Пт 10:00-18:00";

        public ICollection<GroupClass>? GroupClasses { get; set; }
    }
}
