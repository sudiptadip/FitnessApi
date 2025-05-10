using FitnessApi.Data;
using FitnessApi.IRepository;
using FitnessApi.Model;
using System.Diagnostics;

namespace FitnessApi.Repository
{
    public class DailyStepActivityRepository : Repository<DailyStepActivity>, IDailyStepActivityRepository
    {
        private readonly ApplicationDbContext _db;

        public DailyStepActivityRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(DailyStepActivity entity)
        {
            _db.DailyStepActivitys.Update(entity);
            await _db.SaveChangesAsync();
        }
    }
}
