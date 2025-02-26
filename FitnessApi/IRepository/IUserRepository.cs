using FitnessApi.Dto.User;
using FitnessApi.Model;

namespace FitnessApi.IRepository
{
    public interface IUserRepository
    {
        public Task<bool> IsUserExists(string userName);
        Task<APIResponse> Login(LoginRequestDTO loginRequest);
        Task<APIResponse> Register(RegistrationRequestDTO registrationRequest);
    }
}
