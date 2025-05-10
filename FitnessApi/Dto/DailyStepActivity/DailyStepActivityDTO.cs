namespace FitnessApi.Dto.DailyStepActivity
{
    public class DailyStepActivityDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime? Date { get; set; }
        public string Step { get; set; }
        public string Kcal { get; set; }
        public string Km { get; set; }
        public string Minutes { get; set; }
    }
}
