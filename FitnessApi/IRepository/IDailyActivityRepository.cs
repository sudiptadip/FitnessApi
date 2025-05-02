using FitnessApi.Model;

namespace FitnessApi.IRepository
{
    public interface IDailyActivityRepository : IRepository<DailyActivity>
    {
        Task UpdateActivityAsync(DailyActivity activity);
    }
}
