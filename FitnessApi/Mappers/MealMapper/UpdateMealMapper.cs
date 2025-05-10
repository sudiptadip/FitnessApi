using FitnessApi.Dto.Meal;
using FitnessApi.Dto.MealItem;
using FitnessApi.Mappers.IMappers;
using FitnessApi.Model;

namespace FitnessApi.Mappers.MealMapper
{
    public class UpdateMealMapper : IMapper<UpdateMealDTO, Meal>
    {
        public Meal Map(UpdateMealDTO source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return new Meal
            {
                MealName = source.MealName,
                Id = source.Id,
                MealItems = source.MealItems?.Select(item => new MealItem
                {
                    Id = item.Id,
                    Protein = item.Protein,
                    Fat = item.Fat,
                    Carbohydrates = item.Carbohydrates,
                    Kcal = item.Kcal,
                    ServingSize = item.ServingSize,
                    DataSource = item.DataSource,
                    Name = item.Name,
                    ProteinPercentage = item.ProteinPercentage,
                    FatPercentage = item.FatPercentage,
                    CarbohydratePercentage = item.CarbohydratePercentage
                }).ToList() ?? new List<MealItem>()
            };
        }

        public UpdateMealDTO MapBack(Meal destination)
        {
            if (destination == null) throw new ArgumentNullException(nameof(destination));

            return new UpdateMealDTO
            {
                MealName = destination.MealName,
                MealItems = destination.MealItems?.Select(item => new UpdateMealItemDTO
                {
                    Id = item.Id,
                    Protein = item.Protein,
                    Fat = item.Fat,
                    Carbohydrates = item.Carbohydrates,
                    Kcal = item.Kcal,
                    ServingSize = item.ServingSize,
                    DataSource = item.DataSource,
                    Name = item.Name,
                    ProteinPercentage = item.ProteinPercentage,
                    FatPercentage = item.FatPercentage,
                    CarbohydratePercentage = item.CarbohydratePercentage
                }).ToList() ?? new List<UpdateMealItemDTO>()
            };
        }
    }
}
