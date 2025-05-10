using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessApi.Model
{
    public class DailyStepActivity
    {
        public int Id { get; set; }
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public DateTime? Date { get; set; }
        public string Step { get; set; }
        public string Kcal { get; set; }
        public string Km { get; set; }
        public string Minutes { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}
