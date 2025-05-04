using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessApi.Model
{
    public class UserDetail
    {
        public int Id { get; set; }
        [ForeignKey(nameof(ApplicationUser))]
        public string UserId { get; set; }

        public string FitnessGoal { get; set; } 
        public string Gender { get; set; }
        public string Weight { get; set; }
        public string Height { get; set; }
        public bool PreviousFitnessExperience { get; set; }
        public string SpecificDiet { get; set; }
        public int DaysCommit { get; set; }
        public string SpecificExperiencePreferance { get; set; }
        public string CalorieyGoal { get; set; }
        public string SleepQuality { get; set; }

        public string? ProfileImageUrl { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}
