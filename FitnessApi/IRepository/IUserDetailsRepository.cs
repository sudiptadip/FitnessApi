using FitnessApi.Dto.UserDetails;
using FitnessApi.Model;

namespace FitnessApi.IRepository
{
    public interface IUserDetailsRepository : IRepository<UserDetail>
    {
        public Task UpdateUserDetailsAsync(UserDetail entity);
    }
}