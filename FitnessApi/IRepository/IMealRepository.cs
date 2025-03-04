using FitnessApi.Model;

namespace FitnessApi.IRepository
{
    public interface IMealRepository : IRepository<Meal>
    {
        public Task UpdateMealAsync(Meal meal);
    }
}