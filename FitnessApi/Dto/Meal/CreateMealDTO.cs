using FitnessApi.Model;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using FitnessApi.Dto.MealItem;

namespace FitnessApi.Dto.Meal
{
    public class CreateMealDTO
    {
        [Required]
        public string MealName { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public List<CreateMealItemDTO> MealItems { get; set; }
    }
}
