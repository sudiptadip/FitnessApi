using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FitnessApi.Model
{
    public class MealItem
    {
        [Key]
        public int Id { get; set; }
        public decimal Protein { get; set; }
        public decimal Fat { get; set; }
        public decimal Carbohydrates { get; set; }
        public decimal Kcal { get; set; }
        public string ServingSize { get; set; }
        public string DataSource { get; set; }
        public string Name { get; set; }
        public decimal ProteinPercentage { get; set; }
        public decimal FatPercentage { get; set; }
        public decimal CarbohydratePercentage { get; set; }
        [ForeignKey(nameof(Meal))]
        public int MealId { get; set; } 
        public Meal Meal { get; set; }
    }
}
