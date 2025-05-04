using System.ComponentModel.DataAnnotations;

namespace FitnessApi.Dto.UserDetails
{
    public class UpdateUserDetailsDTO
    {
        [Required]
        public string FitnessGoal { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string Weight { get; set; }
        [Required]
        public string Height { get; set; }
        [Required]
        public bool PreviousFitnessExperience { get; set; }
        [Required]
        public string SpecificDiet { get; set; }
        [Required]
        public int DaysCommit { get; set; }
        [Required]
        public string SpecificExperiencePreferance { get; set; }
        [Required]
        public string CalorieyGoal { get; set; }
        [Required]
        public string SleepQuality { get; set; }

        public IFormFile? ProfileImage { get; set; }
    }
}
