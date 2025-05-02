using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FitnessApi.Dto.MealItem
{
    public class MealItemDTO
    {
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
    }
}
