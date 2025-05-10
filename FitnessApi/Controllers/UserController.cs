using FitnessApi.Dto.User;
using FitnessApi.IRepository;
using FitnessApi.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FitnessApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("registration")]
        public async Task<IActionResult> UserRegistration([FromBody] RegistrationRequestDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new APIResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                });
            }

            var response = await _userRepository.Register(model);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("is-user-exists")]
        public async Task<IActionResult> UserExists([FromQuery] string email)
        {
            var response = await _userRepository.IsUserExists(email);

            return StatusCode((int)response.StatusCode, response);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new APIResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                });
            }

            var response = await _userRepository.Login(model);  

            return StatusCode((int)response.StatusCode, response);
        }


        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDTO model)
        {
            var response = await _userRepository.SendOtpForPasswordReset(model.Email);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO model)
        {
            var response = await _userRepository.ResetPasswordWithOtp(model.Email, model.Otp, model.NewPassword);
            return StatusCode((int)response.StatusCode, response);
        }

    }
}