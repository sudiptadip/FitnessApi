using FitnessApi.Model;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using FitnessApi.Dto.MealItem;

namespace FitnessApi.Dto.Meal
{
    public class MealDTO
    {
        public int Id { get; set; }
        public string MealName { get; set; }

        [ForeignKey(nameof(ApplicationUser))]
        public string UserId { get; set; }
        public List<MealItemDTO> MealItems { get; set; }
    }
}
