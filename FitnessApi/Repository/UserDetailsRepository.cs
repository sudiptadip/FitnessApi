using FitnessApi.Data;
using FitnessApi.Dto.UserDetails;
using FitnessApi.IRepository;
using FitnessApi.Model;

namespace FitnessApi.Repository
{
    public class UserDetailsRepository : Repository<UserDetail>, IUserDetailsRepository
    {
        private readonly ApplicationDbContext _db;

        public UserDetailsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateUserDetailsAsync(UserDetail entity)
        {
            _db.UserDetails.Update(entity);
            _db.SaveChanges(); 
        }
    }
}
