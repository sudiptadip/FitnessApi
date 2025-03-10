﻿using System.ComponentModel.DataAnnotations;

namespace FitnessApi.Dto.User
{
    public class LoginRequestDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
