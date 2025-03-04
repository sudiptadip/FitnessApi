using FitnessApi.Dto.Meal;
using FitnessApi.Dto.MealItem;
using FitnessApi.Mappers.IMappers;
using FitnessApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FitnessApi.Mappers.MealMapper
{
    public class CreateMealMappar : IMapper<CreateMealDTO, Meal>
    {
        public Meal Map(CreateMealDTO source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return new Meal
            {
                MealName = source.MealName,
                UserId = source.UserId,
                MealItems = source.MealItems?.Select(item => new MealItem
                {
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

        public CreateMealDTO MapBack(Meal destination)
        {
            if (destination == null) throw new ArgumentNullException(nameof(destination));

            return new CreateMealDTO
            {
                MealName = destination.MealName,
                UserId = destination.UserId,
                MealItems = destination.MealItems?.Select(item => new CreateMealItemDTO
                {
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
                }).ToList() ?? new List<CreateMealItemDTO>()
            };
        }
    }
}
