using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessCenter.Models
{
    public class Visit
    {
        [Key]
        public int VisitID { get; set; }

        [Required]
        [ForeignKey("Client")]
        public int ClientID { get; set; }

        [Display(Name = "Время входа")]
        public DateTime CheckInTime { get; set; }

        [Display(Name = "Время выхода")]
        public DateTime? CheckOutTime { get; set; }

        public virtual Client Client { get; set; }
    }
}
