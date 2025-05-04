using FitnessApi.Data;
using FitnessApi.Dto.User;
using FitnessApi.IRepository;
using FitnessApi.IService;
using FitnessApi.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Generators;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace FitnessApi.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;
        private readonly ILogger<UserRepository> _logger;
        private readonly string _secretKey;
        private readonly IEmailService _emailService;

        public UserRepository(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db,
            IConfiguration configuration,
            ILogger<UserRepository> logger,
            IEmailService emailService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _logger = logger;
            _secretKey = configuration.GetValue<string>("ApiSettings:Secret");
            _emailService = emailService;
        }

        public async Task<bool> IsUserExists(string userName)
        {
            return await _userManager.FindByNameAsync(userName) != null;
        }

        public async Task<APIResponse> Register(RegistrationRequestDTO registrationRequest)
        {
            var response = new APIResponse();

            var user = new ApplicationUser
            {
                UserName = registrationRequest.UserName,
                Email = registrationRequest.UserName,
                NormalizedEmail = registrationRequest.UserName.ToUpper(),
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequest.Password);

                if (!result.Succeeded)
                {
                    _logger.LogWarning("User registration failed: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));

                    response.IsSuccess = false;
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.ErrorMessages = result.Errors.Select(e => e.Description).ToList();
                    return response;
                }

                // Ensure roles exist before assigning
                if (!await _roleManager.RoleExistsAsync("admin"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("admin"));
                    await _roleManager.CreateAsync(new IdentityRole("user"));
                }

                await _userManager.AddToRoleAsync(user, "user");

                response.IsSuccess = true;
                response.StatusCode = HttpStatusCode.OK;
                response.Result = "Successfully user create";

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error during registration: {Message}", ex.Message);

                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.ErrorMessages = new List<string> { "An unexpected error occurred. Please try again later." };
                return response;
            }
        }

        public async Task<APIResponse> Login(LoginRequestDTO loginRequest)
        {
            var response = new APIResponse();

            try
            {
                var user = await _db.ApplicationUser.FirstOrDefaultAsync(u => u.UserName.ToLower() == loginRequest.UserName.ToLower());

                if (user == null)
                {
                    response.IsSuccess = false;
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.ErrorMessages = new List<string> { "Invalid username or password." };
                    return response;
                }

                bool isValid = await _userManager.CheckPasswordAsync(user, loginRequest.Password);

                if (!isValid)
                {
                    response.IsSuccess = false;
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.ErrorMessages = new List<string> { "Invalid username or password." };
                    return response;
                }

                var roles = await _userManager.GetRolesAsync(user);
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_secretKey);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, roles.FirstOrDefault() ?? "user")
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);

                response.IsSuccess = true;
                response.StatusCode = HttpStatusCode.OK;
                response.Result = new LoginResponseDTO
                {
                    Token = tokenHandler.WriteToken(token),
                    UserName = user.UserName,
                    Id = user.Id,
                };

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error during login: {Message}", ex.Message);

                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.ErrorMessages = new List<string> { "An unexpected error occurred. Please try again later." };
                return response;
            }
        }


        public async Task<APIResponse> SendOtpForPasswordReset(string email)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return new APIResponse { IsSuccess = false, StatusCode = HttpStatusCode.NotFound, ErrorMessages = new List<string> { "Email not found" } };
            }

            var otp = new Random().Next(100000, 999999).ToString();
            var otpRecord = new OtpRecord
            {
                Email = email,
                Otp = otp,
                ExpirationTime = DateTime.UtcNow.AddMinutes(10)
            };

            await _db.OtpRecords.AddAsync(otpRecord);
            await _db.SaveChangesAsync();

            _emailService.SendEmail(new SendEmailModel
            {
                To = email,
                Subject = "Password Reset OTP",
                Body = $"Your OTP is: <b>{otp}</b>. It expires in 10 minutes."
            });

            return new APIResponse { IsSuccess = true, StatusCode = HttpStatusCode.OK, Result = "OTP sent to email." };
        }

        public async Task<APIResponse> ResetPasswordWithOtp(string email, string otp, string newPassword)
        {
            var record = await _db.OtpRecords
                .Where(o => o.Email == email && o.Otp == otp && o.ExpirationTime > DateTime.UtcNow)
                .OrderByDescending(o => o.ExpirationTime)
                .FirstOrDefaultAsync();

            if (record == null)
            {
                return new APIResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = new List<string> { "Invalid or expired OTP" }
                };
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new APIResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.NotFound,
                    ErrorMessages = new List<string> { "User not found" }
                };
            }

            // Remove existing password (only works if user has one)
            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (hasPassword)
            {
                var removeResult = await _userManager.RemovePasswordAsync(user);
                if (!removeResult.Succeeded)
                {
                    return new APIResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorMessages = removeResult.Errors.Select(e => e.Description).ToList()
                    };
                }
            }

            // Add new password
            var addResult = await _userManager.AddPasswordAsync(user, newPassword);
            if (!addResult.Succeeded)
            {
                return new APIResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = addResult.Errors.Select(e => e.Description).ToList()
                };
            }

            // Remove used OTP
            _db.OtpRecords.Remove(record);
            await _db.SaveChangesAsync();

            return new APIResponse
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = "Password reset successful."
            };
        }


    }
}