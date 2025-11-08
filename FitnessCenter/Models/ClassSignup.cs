namespace FitnessCenter.Models
{

    public class ClassSignup
    {
        public int ClassSignupID { get; set; }
        public int ClientID { get; set; }
        public Client Client { get; set; } = default!;
        public int GroupClassID { get; set; }
        public GroupClass GroupClass { get; set; } = default!;
        public DateTime SignupDate { get; set; } = DateTime.Now;
        public SignupStatus Status { get; set; } = SignupStatus.Active;
    }
}
