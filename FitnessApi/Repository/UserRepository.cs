using FitnessApi.Data;
using FitnessApi.Dto;
using FitnessApi.IRepository;
using FitnessApi.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
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

        public UserRepository(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db,
            IConfiguration configuration,
            ILogger<UserRepository> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _logger = logger;
            _secretKey = configuration.GetValue<string>("ApiSettings:Secret");
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

    }
}