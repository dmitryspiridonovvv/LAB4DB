namespace FitnessCenter.Models
{
    public class GroupClass
    {
        public int GroupClassID { get; set; }
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public int TrainerID { get; set; }
        public Trainer Trainer { get; set; } = default!;
        public string Schedule { get; set; } = default!;
        public int MaxParticipants { get; set; } = 20;
        public ICollection<ClassSignup>? Signups { get; set; }
    }
}
