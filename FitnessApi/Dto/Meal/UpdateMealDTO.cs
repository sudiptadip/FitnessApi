using FitnessApi.Dto.MealItem;
using System.ComponentModel.DataAnnotations;

namespace FitnessApi.Dto.Meal
{
    public class UpdateMealDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string MealName { get; set; }
        public List<UpdateMealItemDTO> MealItems { get; set; }
    }
}
