using FitnessApi.Data;
using FitnessApi.IRepository;
using FitnessApi.Model;

namespace FitnessApi.Repository
{
    public class DailyActivityRepository : Repository<DailyActivity>, IDailyActivityRepository
    {
        private readonly ApplicationDbContext _db;

        public DailyActivityRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateActivityAsync(DailyActivity activity)
        {
            _db.DailyActivities.Update(activity);
            await _db.SaveChangesAsync();
        }
    }
}
