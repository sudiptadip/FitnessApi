using System.ComponentModel.DataAnnotations;

namespace FitnessApi.Dto.WorkoutImage
{
    public class UpdateWorkoutImageDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string ImageName { get; set; }
        public IFormFile Image { get; set; }
    }
}
