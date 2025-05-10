using System.ComponentModel.DataAnnotations;

namespace FitnessApi.Dto.User
{
    public class RegistrationRequestDTO
    {
        [Required(ErrorMessage = "Username is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string UserName { get; set; }


        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string Password { get; set; }

        [Required]
        public string Type { get; set; }
    }
}