using FitnessApi.Dto.User;
using FitnessApi.Model;

namespace FitnessApi.IRepository
{
    public interface IUserRepository
    {
        public Task<APIResponse> IsUserExists(string userName);
        Task<APIResponse> Login(LoginRequestDTO loginRequest);
        Task<APIResponse> Register(RegistrationRequestDTO registrationRequest);
        Task<APIResponse> SendOtpForPasswordReset(string email);
        Task<APIResponse> ResetPasswordWithOtp(string email, string otp, string newPassword);

    }
}
