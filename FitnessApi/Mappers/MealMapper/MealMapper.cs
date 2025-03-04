using FitnessApi.Dto.Meal;
using FitnessApi.Dto.MealItem;
using FitnessApi.Mappers.IMappers;
using FitnessApi.Model;

namespace FitnessApi.Mappers.MealMapper
{
    public class MealMapper : IMapper<Meal, MealDTO>
    {
        public MealDTO Map(Meal source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return new MealDTO
            {
                Id = source.Id,
                MealName = source.MealName,
                UserId = source.UserId,
                MealItems = source.MealItems?.Select(item => new MealItemDTO
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
                }).ToList() ?? new List<MealItemDTO>()
            };
        }

        public Meal MapBack(MealDTO destination)
        {
            throw new NotImplementedException();
        }
    }
}
