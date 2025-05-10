using System.ComponentModel.DataAnnotations;

namespace FitnessApi.Dto.User
{
    public class LoginRequestDTO
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string UserName { get; set; }
        public string Password { get; set; }
        [Required]
        public string Type { get; set; } 
    }
}
