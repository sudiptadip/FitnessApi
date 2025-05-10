using FitnessApi.Model;

namespace FitnessApi.IRepository
{
    public interface IDailyStepActivityRepository : IRepository<DailyStepActivity>
    {
        Task UpdateAsync(DailyStepActivity entity);
    }
}
