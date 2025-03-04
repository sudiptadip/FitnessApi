using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FitnessApi.Model
{
    public class Meal
    {

        [Key]
        public int Id { get; set; }
        public string MealName { get; set; }

        [ForeignKey(nameof(ApplicationUser))]
        public string UserId { get; set; }
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
        [ValidateNever]
        public List<MealItem> MealItems { get; set; }
    }
}