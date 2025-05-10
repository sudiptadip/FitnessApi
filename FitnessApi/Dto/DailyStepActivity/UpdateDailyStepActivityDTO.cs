namespace FitnessApi.Dto.DailyStepActivity
{
    public class UpdateDailyStepActivityDTO
    {
        public string Step { get; set; }
        public DateTime? Date { get; set; }
        public string Kcal { get; set; }
        public string Km { get; set; }
        public string Minutes { get; set; }
    }
}