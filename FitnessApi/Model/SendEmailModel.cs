using System.ComponentModel.DataAnnotations;

namespace FitnessApi.Model
{
    public class SendEmailModel
    {
        [Required]
        public string To { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Body { get; set; }
    }
}
