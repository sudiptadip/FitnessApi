using System.ComponentModel.DataAnnotations;

namespace FitnessApi.Model
{
    public class WorkoutImage
    {
        public int Id { get; set; }
        public string ImageName { get; set; }
        public string Type { get; set; }
        public string Kg { get; set; }
        public string Sets { get; set; }
        public string Reps { get; set; }
        public string ImageUrl { get; set; }
    }
}