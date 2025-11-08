namespace FitnessCenter.Models
{
    public class MembershipPlan
    {
        public int MembershipPlanID { get; set; }
        public string Name { get; set; } = default!;
        public MembershipKind Kind { get; set; }
        public int DurationDays { get; set; }
        public int? VisitCount { get; set; }
        public decimal Price { get; set; }

        public ICollection<MembershipPlanZone>? PlanZones { get; set; }
        public ICollection<MembershipSale>? Sales { get; set; }
    }
}
