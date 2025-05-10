namespace FitnessApi.Dto.DailyActivity
{
    public class CreateDailyActivityDTO
    {
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public string Water { get; set; }
    }
}