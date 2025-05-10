using System.ComponentModel.DataAnnotations;

namespace FitnessApi.Dto.WorkoutImage
{
    public class UpdateWorkoutImageDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string ImageName { get; set; }

        [Required]
        public string Type { get; set; }
        [Required]
        public string Kg { get; set; }
        [Required]
        public string Sets { get; set; }
        [Required]
        public string Reps { get; set; }
        public IFormFile? Image { get; set; }
    }
}
