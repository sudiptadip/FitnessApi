using FitnessApi.Data;
using FitnessApi.IRepository;
using FitnessApi.Model;
using Microsoft.EntityFrameworkCore;

namespace FitnessApi.Repository
{
    public class MealRepository : Repository<Meal>, IMealRepository
    {
        private readonly ApplicationDbContext _db;
        public MealRepository(ApplicationDbContext db) : base(db) 
        {
            _db = db;
        }

        public async Task UpdateMealAsync(Meal meal)
        {
            using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                var existingMealItems = await _db.MealItems.Where(m => m.MealId == meal.Id).ToListAsync();
                
                var itemsToRemove =  existingMealItems.Where(e => !meal.MealItems.Any(m => m.Id == e.Id)).ToList();

                _db.MealItems.RemoveRange(itemsToRemove);


                foreach (var item in meal.MealItems)
                {
                    var existingItem = existingMealItems.FirstOrDefault(e => e.Id == item.Id);

                    if (existingItem != null)
                    {
                        existingItem.Name = item.Name;
                        existingItem.Protein = item.Protein;
                        existingItem.Fat = item.Fat;
                        existingItem.Carbohydrates = item.Carbohydrates;
                        existingItem.Kcal = item.Kcal;
                        existingItem.ServingSize = item.ServingSize;
                        existingItem.DataSource = item.DataSource;
                        existingItem.ProteinPercentage = item.ProteinPercentage;
                        existingItem.FatPercentage = item.FatPercentage;
                        existingItem.CarbohydratePercentage = item.CarbohydratePercentage;
                    }
                    else
                    {
                        item.MealId = meal.Id;
                        await _db.MealItems.AddAsync(item);
                    }
                }

                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }

        }
    }
}
