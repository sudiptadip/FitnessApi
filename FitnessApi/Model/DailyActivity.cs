using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessApi.Model
{
    public class DailyActivity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public string Water { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
