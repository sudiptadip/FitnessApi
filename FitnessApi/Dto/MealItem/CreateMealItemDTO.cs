using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FitnessApi.Dto.MealItem
{
    public class CreateMealItemDTO
    {
        [Required]
        public decimal Protein { get; set; }
        [Required]
        public decimal Fat { get; set; }
        [Required]
        public decimal Carbohydrates { get; set; }
        [Required]
        public decimal Kcal { get; set; }
        [Required]
        public string ServingSize { get; set; }
        [Required]
        public string DataSource { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal ProteinPercentage { get; set; }
        [Required]
        public decimal FatPercentage { get; set; }
        [Required]
        public decimal CarbohydratePercentage { get; set; }
    }
}
